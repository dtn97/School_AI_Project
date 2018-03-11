#pragma once

#include <iostream>
#include <vector>
#include <stdlib.h>
#include <algorithm>
#include <fstream>
#include <stack>
#include <queue>
#include <set>
using namespace std;

class Graph {

private:
	int n;
	int **a;
	int *heuristic;
	int start;
	int end;
	int* trace;
	vector<int> visit;

	struct Vertex {
		int value1;
		int value2;
		int v;
		Vertex() {
			value1 = value2 = v = 0;
		}
		Vertex(int _a, int _b, int _c) {
			value1 = _b;
			v = _a;
			value2 = _c;
		}
	};

	struct LessThan {
		bool operator () (const Vertex& _a, const Vertex& _b) const {
			if (_a.value1 + _a.value2 == _b.value1 + _b.value2)
				return _a.v > _b.v;
			return _a.value1 + _a.value2 > _b.value1 + _b.value2;
		}
	};

	struct greater {
		bool operator() (const pair<long long, int> a, const pair<long long, int> b) {
			if (a.first == b.first)
				return a.second > b.second;
			return a.first > b.first;
		}
	};


public:
	Graph();
	Graph(const Graph&);
	Graph(int, int**, int*);
	~Graph();

	void set(istream &f);

	void bfs();
	void dfs();
	void ucs();
	void gbfs();
	void astar();
	void print(ostream&);
	void print();
};