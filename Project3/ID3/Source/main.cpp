#include <iostream>
#include <vector>
#include <string>
#include <cstring>
#include <stdlib.h>
#include <fstream>
#include <map>
#include <set>
#include <math.h>
#include <stack>
using namespace std;

vector<string> SplitString(string s){
		vector<string> res;
		char *tmp = new char[s.length() + 1];
		strcpy(tmp, s.c_str());
		char* p = NULL;
		p = strtok(tmp, ", ");
		if (p != NULL)
			res.push_back((string)p);
		while ((p = strtok(NULL, ", ")) != NULL){
			res.push_back((string)p);
		}
		delete[] tmp;
		return res;
	}
class Attribute{
private:
	vector<vector<string> > arr;
	vector<string> AttrName;
	string getAttrName(string s){
		int i = 11;
		int len = s.length();
		while (i < len && s[i] != ' ')
			++i;
		string res = s.substr(11, i - 11);
		return res;
	}
	long double calcEntropy(){
		int sum = arr.size();
		int index = arr[0].size() - 1;
		map<string, int> tmp;
		for (auto i : arr){
			tmp[i[index]]++;
		}
		double entr = 0;
		for (auto i : tmp){
			entr += - (double)i.second / sum * log2((double)i.second / sum);
		}
		return entr;
	}
	long double calcEntropy(int index1, int index2){
		map<string, map<string, int> > cnt;
		map<string, int> tmp;
		int sum = arr.size();
		set<string> s;
		for (auto i : arr){
			++tmp[i[index1]];
			++((cnt[i[index1]])[i[index2]]);
		}
		long double res = 0;
		for (auto i : cnt){
			long double value = 0;
			for (auto j : i.second){
				long double x = (long double)j.second / tmp[i.first];
				value -= x * log2(x);
			}
			res += value * tmp[i.first] / sum;
		}
		return res;
	}

public:
	Attribute(){

	}
	Attribute(const Attribute &p) : arr(p.arr), AttrName(p.AttrName){

	}
	~Attribute(){

	}
	void readInput(string fileName){
		fstream f;
		f.open(fileName, ios::in);
		while (!f.eof()){
			string tmp;
			getline(f, tmp);
			if (tmp == ""){
				getline(f, tmp);
				break;
			}
			AttrName.push_back(this->getAttrName(tmp));
		}
		while (!f.eof()){
			string tmp;
			getline(f, tmp);
			arr.push_back(SplitString(tmp));
		}
		f.close();
	}
	void print(){
		cout << "Attribute name" << endl;
		int j = 0;
		for (auto i : AttrName){
			cout << j++ << ". " << i << endl;
		}
	}
	string getMax(){
		vector<double> tmp;
		int len = arr[0].size() - 1;
		double entro = this->calcEntropy();
		for (int i = 0; i < len; ++i){
			tmp.push_back(entro - this->calcEntropy(i, len));
		}
		int mn = 0;
		for (int i = 1; i < len; ++i){
			if (tmp[i] > tmp[mn])
				mn = i;
		}
		return AttrName[mn];
	}
	set<string> getBestAttribute(string name){
		int index = 0;
		while (AttrName[index] != name)
			++index;
		set<string> res;
		for (auto i : arr){
			res.insert(i[index]);
		}
		return res;
	}
	Attribute getArrtibute(string attName, string name){
		int i = 0;
		while (i < AttrName.size() && AttrName[i] != attName)
			++i;
		Attribute res(*this);
		res.arr.clear();
		res.AttrName.erase(res.AttrName.begin() + i, res.AttrName.begin() + i + 1);
		for (auto j : arr){
			if (j[i] == name){
				res.arr.push_back(j);
			}
		}
		for (int j = 0; j < res.arr.size(); ++j){
			res.arr[j].erase(res.arr[j].begin() + i, res.arr[j].begin() + i + 1);
		}
		return res;
	}
	bool isSameLabel(){
		int len = arr[0].size() - 1;
		int n = arr.size();
		for (int i = 0; i < n - 1; ++i){
			if (arr[i][len] != arr[i + 1][len])
				return false;
		}
		return true;
	}
	pair<string, string> getGoal(){
		int i = arr[0].size() - 1;
		return make_pair(AttrName[i], arr[0][i]);
	}
	void removeAttr(){
		cout << "Nhap vao thuoc tinh ban muon loai bo, nhap -1 neu ban khong muon loai bo thuoc tinh nao: ";
		int x = -1;
		cin >> x;
		getchar();
		if (x >= 0 && x < AttrName.size() - 1){
			AttrName.erase(AttrName.begin() + x, AttrName.begin() + x + 1);
			for (int i = 0; i < arr.size(); ++i){
				arr[i].erase(arr[i].begin() + x, arr[i].begin() + x + 1);
			}
		}
	}
};

struct DecisionTree{
public:
	string name;
	string label;
	vector<DecisionTree*> children;
	DecisionTree(){

	}
	DecisionTree(string a, string b){
		name = a;
		label = b;
	}
	~DecisionTree(){
		int len = children.size();
		for (int i = 0; i < len; ++i)
			delete children[i];
		children.clear();
	}
};

void printTree(DecisionTree* t, int loop = -1){
	if (t == NULL)
		return;
	if (t->label != ""){
		for (int i = 0; i < loop; ++i)
			cout << "|\t";
		cout << "+" << t->name << " = " << t->label << endl;
	}
	for (auto i : t->children){
		if (i != NULL){
			printTree(i, loop + 1);
		}
	}
}

DecisionTree* buildTree(Attribute attr, string a, string b){
	DecisionTree* res = new DecisionTree(a, b);
	if (attr.isSameLabel()){
		pair<string, string> lb = attr.getGoal();
		res->children.push_back(new DecisionTree(lb.first, lb.second));
		return res;
	}
	string name = attr.getMax();
	set<string> arrAttr = attr.getBestAttribute(name);
	for (auto i : arrAttr){
		res->children.push_back(buildTree(attr.getArrtibute(name, i), name, i));
	}
	return res;
}

string query(vector<string> s, DecisionTree *t){
	if (t == NULL)
		return "";
	for (auto i : t->children){
		if (i != NULL){
			for (auto j : s){
				if (i->label == j){
					return query(s, i);
				}
			}
			if (i->children.size() == 0){
				return i->label;
			}
		}
	}
	return "";
}

string Query(string s, DecisionTree *t){
	vector<string> arrStr = SplitString(s);
	int len = arrStr.size() - 1;
	arrStr.erase(arrStr.begin() + len, arrStr.begin() + len + 1);
	return query(arrStr, t);
}

void test(string input, string compare, DecisionTree *t){
	fstream f1;
	f1.open(input, ios::in);
	fstream f2;
	f2.open(compare, ios::in);
	int cnt = 0, sum = 0;

	while (!f1.eof() && !f2.eof()){
		string s1;
		string s2;
		getline(f1, s1);
		getline(f2, s2);
		string s = Query(s1, t);
		cout << "Input: " << s1 << endl;
		cout << "Output " << s << endl;
		cout << "Result " << s2 << endl << endl;
		++sum;
		if (s == s2)
			++cnt;
	}
	f1.close();
	f2.close();
	cout << "Da test " << sum << " mau, ket qua chinh xac " << cnt << " mau." << endl;
	cout << "Do chinh xac: " << (double)cnt * 100 / sum << " %" << endl;
}

int main(){
	Attribute attr;
	attr.readInput("./train.txt");
	attr.print();
	attr.removeAttr();
	DecisionTree *t = buildTree(attr, "", "");
	cout << endl << endl;
	printTree(t);
	cout << endl << endl;
	test("./test.txt", "./compare.txt", t);

	return 0;
}