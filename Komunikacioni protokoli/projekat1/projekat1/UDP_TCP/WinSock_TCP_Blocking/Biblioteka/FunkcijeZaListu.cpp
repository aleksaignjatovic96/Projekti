#include "StudentLista.h"
#include <malloc.h>


void dodajNaKraj(struct StudentLista **glava, Student studentNovi)
{
	StudentLista *clan = (StudentLista*)(malloc(sizeof(StudentLista)));
	clan->student.id = studentNovi.id;
	clan->student.ime = studentNovi.ime;
	clan->student.prezime = studentNovi.prezime;
	clan->sledeci = NULL;

	if (*glava == NULL)
	{
		*glava = clan;
		return;
	}

	StudentLista *tekuci = *glava;

	while (tekuci->sledeci != NULL)
	{
		tekuci = tekuci->sledeci;
	};

	tekuci->sledeci = clan;

}

void dodajNaPocetak(struct StudentLista **glava, Student studentNovi)
{
	StudentLista *clan = (StudentLista*)(malloc(sizeof(StudentLista)));
	clan->student.id = studentNovi.id;
	clan->student.ime = studentNovi.ime;
	clan->student.prezime = studentNovi.prezime;
	clan->sledeci = NULL;

	if (*glava == NULL)
	{
		*glava = clan;
		//printf("Student %s %s sa ID: %d je dodat!\n-------------------------------------\n", studentNovi.ime, studentNovi.prezime, studentNovi.id);
		return;
	}

	StudentLista *tekuci = *glava;


		clan->sledeci = *glava;
		*glava = clan;
		//printf("Student %s %s sa ID: %d je dodat!\n-------------------------------------\n", studentNovi.ime, studentNovi.prezime, studentNovi.id);
		return;

}

void ispisiListu(struct StudentLista **glava)
{

	StudentLista *tekuci = *glava;


	if (*glava == NULL)
	{
		printf("Lista je prazna!\n");
		return;
	}

	while (tekuci != NULL)
	{
		printf("----------------\n");
		printf("Student ID: %d\n", tekuci->student.id);
		printf("Ime: %s\n", tekuci->student.ime);
		printf("Prezime: %s\n", tekuci->student.prezime);

		tekuci = tekuci->sledeci;
	}

}

void dodajUSredinu(struct StudentLista **glava, Student studentNovi)
{
	StudentLista *clan = (StudentLista*)(malloc(sizeof(StudentLista)));
	clan->student.id = studentNovi.id;
	clan->student.ime = studentNovi.ime;
	clan->student.prezime = studentNovi.prezime;
	clan->sledeci = NULL;

	if (*glava == NULL)
	{
		*glava = clan;
		printf("Student %s %s sa ID: %d je dodat!\n-------------------------------------\n", studentNovi.ime, studentNovi.prezime, studentNovi.id);
		return;
	}

	StudentLista *tekuci = *glava;
	StudentLista *prethodni = NULL;

	//Dodavanje na pocetak na mesto glave
	if (clan->student.id < tekuci->student.id)
	{
		clan->sledeci = *glava;
		*glava = clan;
		printf("Student %s %s sa ID: %d je dodat!\n-------------------------------------\n", studentNovi.ime, studentNovi.prezime, studentNovi.id);
		return;
	}

	if ((*glava)->sledeci == NULL)
	{
		(*glava)->sledeci = clan;
		return;
	}

	prethodni = tekuci;
	tekuci = tekuci->sledeci;

	while (tekuci != NULL)
	{
		//Da li je novi manji od tekuceg
		if (clan->student.id < tekuci->student.id)
		{
			clan->sledeci = tekuci;
			tekuci = clan;
			prethodni->sledeci = tekuci;
			printf("Student %s %s sa ID: %d je dodat!\n-------------------------------------\n", studentNovi.ime, studentNovi.prezime, studentNovi.id);
			return;
		}

		//Nemoguce dodavanje, isti ID
		else if (clan->student.id == tekuci->student.id)
		{
			printf("Student sa ID: %d vec postoji!\nNe moze se ubaciti student %s %s!\n-------------------------------------\n", studentNovi.id, studentNovi.ime, studentNovi.prezime);
			break;
		}
		

		//Nastavak sortiranja
		else if (clan->student.id > tekuci->student.id)
		{
			prethodni = tekuci;
			tekuci = tekuci->sledeci;

			if (tekuci == NULL)
			{
				printf("Student %s %s sa ID: %d je dodat!\n-------------------------------------\n", studentNovi.ime, studentNovi.prezime, studentNovi.id);
				prethodni->sledeci = clan;
			}

		}

	};
}

void izbrisiStudenta(struct StudentLista **glava, int id)
{
	if (*glava == NULL)
	{
		printf("Lista je prazna!\n");
		return;
	}

	StudentLista *tekuci = *glava;
	StudentLista *prethodni = NULL;



	//Brisanje glave
	if (id == (*glava)->student.id)
	{		
		*glava = (*glava)->sledeci;
		printf("\n-----------------\nIzbrisan je student %s %s!\n-------------------------------------\n", tekuci->student.ime, tekuci->student.prezime);
		free(tekuci);
		return;
	}

	prethodni = tekuci;
	tekuci = tekuci->sledeci;

	while (tekuci != NULL)
	{
		if (id == tekuci->student.id)
		{
			prethodni->sledeci = tekuci->sledeci;
			printf("\n-------------------------------------\nIzbrisan je student %s %s!\n-------------------------------------\n", tekuci->student.ime, tekuci->student.prezime);
			free(tekuci);

			return;
		}

		prethodni = tekuci;
		tekuci = tekuci->sledeci;
	}

	printf("Ne postoji student sa ID %d\n-------------------------------------\n", id);
}

int racunajDuzinuListe(struct StudentLista **glava)
{

	int rez = 0;

	if (*glava == NULL)
	{
		printf("Lista je prazna!\n");
		return 0;
	}

	StudentLista *tekuci = *glava;


	while (tekuci != NULL)
	{
		rez += sizeof(tekuci->student.id) + strlen(tekuci->student.ime) + strlen(tekuci->student.prezime);



		tekuci = tekuci->sledeci;
	}


	return rez;
}

int * racunajVelicinuPoljaUStudentu(struct StudentLista **glava)
{

	int prolaza = vratiBrojBajtovaUNizu(glava);

	StudentLista *tekuci = *glava;
	int *niz = (int*)malloc(prolaza*sizeof(int));
	tekuci = *glava;

	for (int i = 0; i < prolaza; i+=3)
	{
		niz[i] = (int)sizeof(tekuci->student.id);
		niz[i + 1] = strlen(tekuci->student.ime);
		niz[i + 2] = strlen(tekuci->student.prezime);

		tekuci = tekuci->sledeci;
	}

	return niz;

}

int vratiBrojBajtovaUNizu(struct StudentLista **glava)
{
	int prolaza = 0;

	if (*glava == NULL)
	{
		printf("Lista je prazna!\n");
		return 0;
	}

	StudentLista *tekuci = *glava;


	while (tekuci != NULL)
	{
		prolaza += 3;

		tekuci = tekuci->sledeci;
	}

	return prolaza;
}

void serijalizacija(StudentLista **glava, char *buffer, int *niz, int duzina, int duzinaNiza)
{
	if (*glava == NULL)
	{
		printf("Lista je prazna!\n");
		return;
	}

	StudentLista *tekuci = *glava;
	int pomeraj = 0;
	int i = 0;

	while (tekuci != NULL)
	{




		memcpy(buffer + pomeraj, (int *)&tekuci->student.id, sizeof(tekuci->student.id));
		pomeraj += sizeof(tekuci->student.id);
		i++;


		char *ime = (char*)malloc(*(niz + i));
		ime = tekuci->student.ime;
		memcpy(buffer + pomeraj, ime, *(niz + i));
		pomeraj += *(niz + i);
		i++;

		char *prezime = (char*)malloc(*(niz + i));
		prezime = tekuci->student.prezime;
		memcpy(buffer + pomeraj, prezime, *(niz + i));
		pomeraj += *(niz + i);
		i++;


		tekuci = tekuci->sledeci;
	}

	int j = 0;

	for (int i = pomeraj; i < duzina + duzinaNiza; i += 4)
	{
		memcpy(buffer + pomeraj, &niz[j], sizeof(&niz[j]));
		pomeraj += sizeof(&niz[j]);
		j++;
	}


	memcpy(buffer + pomeraj, &duzina, sizeof(duzina));

}

void deserijalizacija(StudentLista **glava, char *bufferDuzina, char *buffer)
{
	
	int duzina = *(int*)((char*)buffer + (*(int*)bufferDuzina - 4));
	int *niz = (int*)(buffer + duzina);

	struct Student *noviStudent = (struct Student*)malloc(sizeof(struct Student));


	int pomeraj = 0;
	int i = 0;

	do
	{



	    memcpy(&noviStudent->id, buffer + pomeraj, sizeof(int));
		pomeraj += *(niz + i);
		i++;

		char *ime = (char*)calloc(*(niz + i), *(niz + i));
		memcpy(ime, buffer + pomeraj, *(niz + i));
		noviStudent->ime = ime;
		pomeraj += *(niz + i);
		i++;


		char *prezime = (char*)calloc(*(niz + i), *(niz + i));
		memcpy(prezime, buffer + pomeraj, *(niz + i));
		noviStudent->prezime = prezime;
		pomeraj += *(niz + i);
		i++;

		dodajNaPocetak(glava, *noviStudent);

	} while (pomeraj != duzina);

}

//------------------------------------------------------------------------------------------------- NITI

KriticnaSekcija * inicijalizacija()
{
	KriticnaSekcija* ks = (KriticnaSekcija*)malloc(sizeof(KriticnaSekcija));
	ks->glava = NULL;
	InitializeCriticalSection(&ks->cs);
	return ks;
}

void dodajUSredinuNit(struct KriticnaSekcija **ks, struct Student studentNovi)
{
	StudentLista *clan = (StudentLista*)(malloc(sizeof(StudentLista)));
	clan->student.id = studentNovi.id;
	clan->student.ime = studentNovi.ime;
	clan->student.prezime = studentNovi.prezime;
	clan->sledeci = NULL;

	if ((*ks)->glava == NULL)
	{
		EnterCriticalSection(&(*ks)->cs);
		(*ks)->glava = clan;
		printf("Student %s %s sa ID: %d je dodat!\n-------------------------------------\n", studentNovi.ime, studentNovi.prezime, studentNovi.id);
		LeaveCriticalSection(&(*ks)->cs);
		return;
	}

	EnterCriticalSection(&(*ks)->cs);
	StudentLista *tekuci = (*ks)->glava;
	StudentLista *prethodni = NULL;
	LeaveCriticalSection(&(*ks)->cs);

	//Dodavanje na pocetak na mesto glave
	if (clan->student.id < tekuci->student.id)
	{
		EnterCriticalSection(&(*ks)->cs);
		clan->sledeci = (*ks)->glava;
		(*ks)->glava = clan;
		printf("Student %s %s sa ID: %d je dodat!\n-------------------------------------\n", studentNovi.ime, studentNovi.prezime, studentNovi.id);
		LeaveCriticalSection(&(*ks)->cs);
		return;
	}

	if (((*ks)->glava)->sledeci == NULL)
	{
		EnterCriticalSection(&(*ks)->cs);
		((*ks)->glava)->sledeci = clan;
		printf("Student %s %s sa ID: %d je dodat!\n-------------------------------------\n", studentNovi.ime, studentNovi.prezime, studentNovi.id);
		LeaveCriticalSection(&(*ks)->cs);
		return;
	}

	//EnterCriticalSection(&(*ks)->cs);
	prethodni = tekuci;
	tekuci = tekuci->sledeci;
	//EnterCriticalSection(&(*ks)->cs);

	while (tekuci != NULL)
	{
		//Da li je novi manji od tekuceg
		if (clan->student.id < tekuci->student.id)
		{
			EnterCriticalSection(&(*ks)->cs);
			clan->sledeci = tekuci;
			tekuci = clan;
			prethodni->sledeci = tekuci;
			printf("Student %s %s sa ID: %d je dodat!\n-------------------------------------\n", studentNovi.ime, studentNovi.prezime, studentNovi.id);
			LeaveCriticalSection(&(*ks)->cs);
			return;
		}

		//Nemoguce dodavanje, isti ID
		else if (clan->student.id == tekuci->student.id)
		{
			printf("Student sa ID: %d vec postoji!\nNe moze se ubaciti student %s %s!\n-------------------------------------\n", studentNovi.id, studentNovi.ime, studentNovi.prezime);
			break;
		}


		//Nastavak sortiranja
		else if (clan->student.id > tekuci->student.id)
		{
			EnterCriticalSection(&(*ks)->cs);
			prethodni = tekuci;
			tekuci = tekuci->sledeci;

			if (tekuci == NULL)
			{
				printf("Student %s %s sa ID: %d je dodat!\n-------------------------------------\n", studentNovi.ime, studentNovi.prezime, studentNovi.id);
				prethodni->sledeci = clan;
			}
			LeaveCriticalSection(&(*ks)->cs);

		}

	};
	
}

int izbrisiStudentaNit(struct KriticnaSekcija **ks, int id)
{
	if ((*ks)->glava == NULL)
	{
		printf("=========================\nLista je prazna!\nNe postoji student sa ID %d\n=========================\n", id);
		return -1;
	}

	EnterCriticalSection(&(*ks)->cs);
	StudentLista *tekuci = (*ks)->glava;
	StudentLista *prethodni = NULL;
	LeaveCriticalSection(&(*ks)->cs);


	//Brisanje glave
	if (id == ((*ks)->glava)->student.id)
	{
		EnterCriticalSection(&(*ks)->cs);
		(*ks)->glava = ((*ks)->glava)->sledeci;
		printf("\n-------------------------------------\nIzbrisan je student %s %s %d!\n-------------------------------------\n", tekuci->student.ime, tekuci->student.prezime, tekuci->student.id);
		free(tekuci);
		LeaveCriticalSection(&(*ks)->cs);
		return 1;
	}

	//EnterCriticalSection(&(*ks)->cs);
	prethodni = tekuci;
	tekuci = tekuci->sledeci;
	//LeaveCriticalSection(&(*ks)->cs);

	while (tekuci != NULL)
	{
		EnterCriticalSection(&(*ks)->cs);

		if (id == tekuci->student.id)
		{

			prethodni->sledeci = tekuci->sledeci;
			printf("\n-------------------------------------\nIzbrisan je student %s %s %d!\n-------------------------------------\n", tekuci->student.ime, tekuci->student.prezime, tekuci->student.id);
			free(tekuci);
			LeaveCriticalSection(&(*ks)->cs);
			return 1;
		}

		prethodni = tekuci;
		tekuci = tekuci->sledeci;

		LeaveCriticalSection(&(*ks)->cs);
	}

	printf("Ne postoji student sa ID %d\n-------------------------------------\n", id);
	return 0;
}

DWORD WINAPI nitDodaj(LPVOID KSParam)
{
	KriticnaSekcija *ks = (KriticnaSekcija*)KSParam;

	int i = 2;
	while (true)
	{
		Student stud;
		stud.id = i;
		stud.ime = "StudentIme";
		stud.prezime = "StudentPrezime";

		dodajUSredinuNit(&ks, stud);
		i++;
		Sleep(1500);
	}

	return 0;
}

DWORD WINAPI nitIbrisi(LPVOID KSParam)
{
	KriticnaSekcija *ks = (KriticnaSekcija*)KSParam;
	int i = 0;

	while(true)
	{

		int k = izbrisiStudentaNit(&ks, i);

		if (k == 0)
		{
			Sleep(1300);
			continue;
		}

		else if (k == -1)
		{
			Sleep(1300);
			continue;
		}

		else if (k == 1)
		{
			i++;
			Sleep(1300);
			continue;
		}

	}

	return 0;
}


