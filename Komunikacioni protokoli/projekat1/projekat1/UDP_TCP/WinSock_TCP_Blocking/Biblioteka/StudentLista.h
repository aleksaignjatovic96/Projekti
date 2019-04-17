#pragma once
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <windows.h>

struct Student
{
	int id;
	char *ime;
	char *prezime;
};

struct StudentLista {

	struct Student student;
	struct StudentLista *sledeci;

};

struct KriticnaSekcija
{
	StudentLista *glava;
	CRITICAL_SECTION cs;
};

void dodajNaKraj(struct StudentLista **, struct Student);
void dodajNaPocetak(struct StudentLista **, struct Student);
void dodajUSredinu(struct StudentLista **, struct Student);
void ispisiListu(struct StudentLista **);
void izbrisiStudenta(struct StudentLista **, int);
int racunajDuzinuListe(struct StudentLista **);
int *racunajVelicinuPoljaUStudentu(struct StudentLista **);
int vratiBrojBajtovaUNizu(struct StudentLista **);
void serijalizacija(StudentLista **, char *, int *, int, int);
void deserijalizacija(StudentLista **, char *, char*);
//niti
KriticnaSekcija *inicijalizacija();
void dodajUSredinuNit(struct KriticnaSekcija **, struct Student);
int izbrisiStudentaNit(struct KriticnaSekcija **, int);
DWORD WINAPI nitDodaj(LPVOID);
DWORD WINAPI nitIbrisi(LPVOID);