#include "Graph.h"

int main(int argc, char** argv) {
	if (argc != 2)
		return -1;

	fstream fin;
	fin.open(argv[1], ios_base::in);

	Graph g;
	g.set(fin);
	fin.close();
	g.print();

	return 0;
}