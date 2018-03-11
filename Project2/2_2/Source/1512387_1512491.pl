male(queenElizabethII).
male(princeCharles).
male(captainPhillips).
male(timothyLaurence).
male(princeAndrew).
male(princeEdward).
male(princeWilliam).
male(princeHarry).
male(peterPhillips).
male(mikeTindall).
male(jamesSevern).
male(princeGeorge).
male(theNewPrincess).
male(miaTindall).

female(autumnKelly).
female(kateMiddleton).
female(sophieRhys-jones).
female(princessAnne).
female(camillaBowles).
female(pricePhilip).
female(princessDiana).
female(sarahFerguson).
female(zaraPhillips).
female(princessBeatrice).
female(princessEngenie).
female(ladyLouise).
female(savannahPhillips).
female(islaPhillips).

parent(queenElizabethII, princeCharles).
parent(queenElizabethII, princessAnne).
parent(queenElizabethII, princeAndrew).
parent(queenElizabethII, princeEdward).
parent(princePhillip, princeCharles).
parent(princePhillip, princessAnne).
parent(princePhillip, princeAndrew).
parent(princePhillip, princeEdward).
parent(princessDiana, princeWilliam).
parent(princessDiana, princeHarry).
parent(princeCharles, princeWilliam).
parent(princeCharles, princeHarry).
parent(captainPhillips, peterPhillips).
parent(captainPhillips, zaraPhillips).
parent(princessAnne, peterPhillips).
parent(princessAnne, zaraPhillips).
parent(sarahFerguson, princessBeatrice).
parent(sarahFerguson, princessEugenie).
parent(princeAndrew, princessBeatrice).
parent(princeAndrew, princessEugenie).
parent(sophieRhys-joins, jamesSevern).
parent(sophieRhys-joins, ladyLouise).
parent(princeEdward, jamesSevern).
parent(princeEdward, ladyLouise).
parent(princeWilliam, princeGeorge).
parent(princeWilliam, theNewPrincess).
parent(kateMiddleton, princeGeorge).
parent(kateMiddleton, theNewPrincess).
parent(autumnKelly, savannahPhillips).
parent(autumnKelly, islaPhillips).
parent(peterPhillips, savannahPhillips).
parent(peterPhillips, islaPhillips).
parent(zaraPhillips, miaTindall).
parent(mikeTindall, miaTindall).

father(Parent, Child) :- parent(Parent, Child), male(Parent).
mother(Parent, Child) :- parent(Parent, Child), female(Parent).
child(Child, Parent) :- parent(Parent, Child).
son(Child, Parent) :- child(Child,Parent), male(Child).
daughter(Child, Parent) :- child(Child, Parent), female(Child).
grandparent(GP, GC) :- parent(GP, X), parent(X, GC).
grandmother(GM, GC) :- parent(GM, X), parent(X, GC), female(GM).
grandfather(GF, GC) :- parent(GF, X), parent(X, GC), male(GF).
grandchild(GC, GP) :- grandParent(GP, GC).
grandson(GS, GP) :- grandChild(GS, GP), male(GS).
granddaughter(GD, GP) :- grandchild(GD, GP), female(GD).

married(queenElizabethII, princePhillip).
married(princeCharles, camillaBowles).
married(princessAnne, timothyLaurence).
married(sophieRhys-joins, princeEdward).
married(princeWilliam, kateMiddleton).
married(autumnKelly, peterPhillips).
married(zaraPhillips, mikeTindall).

married(mikeTindall, zaraPhillips).
married(peterPhillips, autumnKelly).
married(kateMiddleton, princeWilliam).
married(princeEdward, sophieRhys-joins).
married(timothyLaurence, princessAnne).
married(camillaBowles, princeCharles).
married(princePhillip, queenElizabethII).

married(princessDiana, princeCharles).
married(princeCharles, princessDiana).
married(captainPhillips, princessAnne).
married(princessAnne, captainPhillips).
married(sarahFerguson, princeAndrew).
married(princeAndrew, sarahFerguson).

divorced(princessDiana, princeCharles).
divorced(princeCharles, princessDiana).
divorced(captainPhillips, princessAnne).
divorced(princessAnne, captainPhillips).
divorced(sarahFerguson, princeAndrew).
divorced(princeAndrew, sarahFerguson).


husband(Person, Wife) :- married(Person, Wife), not(divorced(Person, Wife)), male(Person).
wife(Person, Husband) :- married(Person, Husband), not(divorced(Person, Husband)), female(Person).
sibling(Person1, Person2) :- parent(X, Person1), parent(X, Person2).
brother(Person, Sibling) :- sibling(Person, Sibling), male(Person).
sister(Person, Sibling) :- sibling(Person, Sibling), female(Person).
aunt(Aunt, Person) :- (sister(Aunt, X), parent(X, Person));(wife(Aunt,X), sibling(X,Y), parent(Y, Person)).
uncle(Uncle, Person) :- (brother(Uncle, X), parent(X, Person));(husband(Uncle,X), sibling(X,Y),parent(Y,Person)).
nephew(Nephew, Person):-(son(Nephew,X),sibling(X,Person));(son(Nephew,X),sibling(X,Y),married(Y,Person)), not(divorced(Y, Person)).
niece(Niece, Person):-(daughter(Niece,X),sibling(X,Person));(daughter(Niece,X),sibling(X,Y),married(Y,Person)), not(divorced(Y, Person)).

numeral(0).
numeral(succ(X)) :- numeral(X).

child1(anne,bridget).
child1(bridget,caroline).
child1(caroline,donna).
child1(donna,emily).

descend(X,Y) :- child1(X,Y).

descend(X,Y) :- child1(X,Z),
                 descend(Z,Y).

