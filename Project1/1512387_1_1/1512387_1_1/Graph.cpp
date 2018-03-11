#include "Graph.h"

Graph::Graph() {
	n = 0;
	a = NULL;
	start = end = 0;
	heuristic = NULL;
	trace = NULL;
}

Graph::Graph(const Graph& p) {
	n = p.n;
	a = new int*[n];
	for (int i = 0; i < n; ++i) {
		a[i] = new int[n];
		for (int j = 0; j < n; ++j)
			a[i][j] = p.a[i][j];
	}
	heuristic = new int[n];
	for (int i = 0; i < n; ++i)
		heuristic[i] = p.heuristic[i];
	start = p.start;
	end = p.end;
}

Graph::Graph(int pn, int **pa, int *pheuristic) {
	n = pn;
	a = new int*[n];
	for (int i = 0; i < n; ++i) {
		a[i] = new int[n];
		for (int j = 0; j < n; ++j)
			a[i][j] = pa[i][j];
	}
	heuristic = new int[n];
	for (int i = 0; i < n; ++i)
		heuristic[i] = pheuristic[i];
}

Graph::~Graph() {
	for (int i = 0; i < n; ++i)
		delete[]a[i];
	delete[] a;
	delete[] heuristic;
	delete[] trace;
	a = NULL;
	heuristic = NULL;
	trace = NULL;
}

void Graph::set(istream &f) {
	if (n != 0) {
		for (int i = 0; i < n; ++i) {
			delete[] a[i];
			a[i] = NULL;
		}
		delete[] a;
		a = NULL;
		delete[] heuristic;
		heuristic = NULL;
		n = 0;
	}

	f >> n >> start >> end;

	a = new int*[n];
	for (int i = 0; i < n; ++i) {
		a[i] = new int[n];
		for (int j = 0; j < n; ++j)
			f >> a[i][j];
	}

	heuristic = new int[n];
	for (int i = 0; i < n; ++i)
		f >> heuristic[i];

	trace = new int[n];
}

void Graph::bfs() {
	visit.clear();
	queue<int> arr;
	memset(trace, -1, n * sizeof(int));
	trace[start] = start;
	arr.push(start);
	while (!arr.empty()) {
		int u = arr.front();
		arr.pop();
		visit.push_back(u);
		if (u == end)
			return;
		for (int i = 0; i < n; ++i) {
			if (a[u][i] > 0 && trace[i] == -1) {
				trace[i] = u;
				arr.push(i);
			}
		}
	}
}

void Graph::dfs() {
	stack<int> arr;
	memset(trace, -1, n * sizeof(int));
	trace[start] = start;
	arr.push(start);
	visit.clear();
	bool *check = new bool[n];
	memset(check, 1, n);
	while (!arr.empty()) {
		int u = arr.top();
		arr.pop();
		if (!check[u])
			continue;
		visit.push_back(u);
		check[u] = false;
		if (u == end)
			return;
		for (int i = n - 1; ~i; --i) {
			if (a[u][i] > 0) {
				if (check[i] == false)
					continue;
				trace[i] = u;
				arr.push(i);
			}
		}
	}
}

void Graph::ucs() {
	visit.clear();
	typedef long long ll;
	visit.clear();
	memset(trace, -1, n * sizeof(int));
	priority_queue<pair<ll, int>, vector<pair<ll, int> >, greater> arr;
	arr.push(make_pair(0, start));
	vector<ll> d;
	d.assign(n, 1e17);
	d[start] = 0;
	trace[start] = start;

	while (!arr.empty()) {
		ll w = arr.top().first;
		int u = arr.top().second;
		arr.pop();
		if (d[u] != w)
			continue;
		visit.push_back(u);
		if (u == end)
			return;
		for (int v = 0; v < n; ++v) {
			if (a[u][v] > 0 && d[v] > d[u] + a[u][v]) {
				d[v] = d[u] + a[u][v];
				trace[v] = u;
				arr.push(make_pair(d[v], v));
			}
		}
	}
	for (auto i : visit)
		cout << i << "  ";
	cout << endl;
}


void Graph::gbfs() {

	visit.clear();
	memset(trace, -1, n * sizeof(int));
	priority_queue<Vertex, vector<Vertex>, LessThan> arr;
	arr.push(Vertex(start, 0, 0));
	bool *check = new bool[n];
	memset(check, 1, n);
	while (!arr.empty()) {
		Vertex u = arr.top();
		arr.pop();
		check[u.v] = false;
		visit.push_back(u.v);
		if (u.v == end)
			return;
		for (int i = 0; i < n; ++i) {
			if (check[i]) {
				if (a[u.v][i] > 0) {
					trace[i] = u.v;
					arr.push(Vertex(i, 0, heuristic[i]));
				}
			}

		}
	}
}

void Graph::astar() {
	visit.clear();
	memset(trace, -1, n * sizeof(int));
	trace[start] = start;
	priority_queue<pair<long long, int>, vector<pair<long long, int> >, greater > arr;
	arr.push(make_pair(heuristic[start], start));
	vector<long long> d;
	d.assign(n, 1e17);
	d[start] = 0;
	while (!arr.empty()) {
		int u = arr.top().second;
		long long w = arr.top().first - heuristic[u];
		arr.pop();
		if (d[u] != w)
			continue;
		visit.push_back(u);
		if (u == end) {
			return;
		}
		for (int v = 0; v < n; ++v) {
			if (a[u][v] > 0) {
				if (d[v] > d[u] + a[u][v]) {
					d[v] = d[u] + a[u][v];
					trace[v] = u;
					arr.push(make_pair((d[v] + heuristic[v]), v));
				}
			}

		}
	}
}

void Graph::print(ostream& os) {
	if (trace[end] == -1) {
		cout << "Khong tim thay duong di" << endl;
		return;
	}

	int s = start;
	int f = end;

	stack<int> arr;

	while (s != f) {
		arr.push(f);
		if (f < 0)
			return;
		f = trace[f];
	}
	arr.push(f);

	for (auto i : visit) {
		os << i << "  ";
	}
	os << endl;
	while (!arr.empty()) {
		os << arr.top() << "  ";
		arr.pop();
	}

	os << endl;
}

void Graph::print() {
	char fileOut[][10] = { "bfs.txt", "dfs.txt", "ucs.txt", "gbfs.txt", "astar.txt" };
	fstream f;

	f.open(fileOut[0], ios_base::out);
	this->bfs();
	this->print(f);
	f.close();
	
	f.open(fileOut[1], ios_base::out);
	this->dfs();
	this->print(f);
	f.close();
	
	f.open(fileOut[2], ios_base::out);
	this->ucs();
	this->print(f);
	f.close();

	f.open(fileOut[3], ios_base::out);
	this->gbfs();
	this->print(f);
	f.close();

	f.open(fileOut[4], ios_base::out);
	this->astar();
	this->print(f);
	f.close();

}