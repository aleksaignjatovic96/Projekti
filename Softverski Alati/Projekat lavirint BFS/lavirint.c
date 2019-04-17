#include "stdafx.h"
#include <stdlib.h>


#define MAXLAVIRINT 12

int *a, *b, ukTalas;
char  L[13][13] = { 
// 0    1    2    3    4    5    6    7    8    9   10   11   12
{ 'Z', 'Z', 'Z', 'Z', 'Z', 'Z', 'Z', 'Z', 'Z', 'Z', 'Z', 'S', 'Z' },  // 0
{ 'Z', '.', 'Z', '.', '.', '.', '.', '.', '.', '.', '.', '.', 'Z' },  // 1 
{ 'Z', '.', 'Z', '.', 'Z', 'Z', '.', 'Z', 'Z', 'Z', 'Z', 'Z', 'Z' },  // 2
{ 'Z', '.', 'Z', '.', 'Z', 'Z', '.', 'Z', '.', '.', '.', '.', 'Z' },  // 3
{ 'Z', '.', 'Z', '.', '.', 'Z', '.', 'Z', '.', 'Z', 'Z', '.', 'Z' },  // 4
{ 'Z', '.', 'Z', 'Z', 'Z', 'Z', '.', 'Z', 'Z', 'Z', 'Z', '.', 'Z' },  // 5
{ 'Z', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', 'Z' },  // 6
{ 'Z', '.', 'Z', 'Z', 'Z', 'Z', '.', 'Z', 'Z', 'Z', 'Z', '.', 'Z' },  // 7
{ 'Z', '.', 'Z', 'Z', 'K', 'Z', '.', 'Z', '.', '.', 'Z', '.', 'Z' },  // 8
{ 'Z', '.', '.', '.', '.', 'Z', '.', 'Z', 'Z', '.', 'Z', '.', 'Z' },  // 9
{ 'Z', 'Z', 'Z', 'Z', 'Z', 'Z', '.', 'Z', 'Z', '.', 'Z', '.', 'Z' },  // 10
{ 'Z', '.', '.', '.', '.', '.', '.', '.', '.', '.', 'Z', '.', 'Z' },  // 11
{ 'Z', 'Z', 'Z', 'Z', 'Z', 'Z', 'Z', 'Z', 'Z', 'Z', 'Z', 'Z', 'Z' } };// 12

struct red{
	int talas;
	int x, y;
	struct red *sledeci;
	struct red *predhodni;
};

void prikazilavirint()
{
	system("@cls||clear");

	int i, j;

	for (i = 0; i < 13; i++)
	{
		for (j = 0; j < 13; j++)
		{
			char tacka = L[i][j];
			if (tacka == 'Z')
			{
				printf("%c", 219);
				printf("%c", 219);
			}
			else if (tacka == '.')
			{
				printf(" %c", 248);
			}
			else if (tacka == 'K')
			{
				printf(" %c", 1);
			}
			else
			{
				printf("%2d", tacka);
			}
		}

		printf("%c", '\n');
	}
}

struct red *dodajCvor(struct red *_ispitajRed, int x, int y, int _krug){
	struct red *temp;



	if (_ispitajRed == NULL){
		_ispitajRed = (struct red *) malloc(sizeof(struct red)); /* insert the new node first node*/


		if (_ispitajRed == NULL){
			printf("Nije uspelo rezervisanje memorije\n");
			exit(0);
		}

		_ispitajRed->talas = _krug;
		_ispitajRed->x = x;
		_ispitajRed->y = y;
		_ispitajRed->sledeci = NULL;
		_ispitajRed->predhodni = NULL;
	}
	else {
		temp = _ispitajRed;

		// trazi zadnjeg u redu
		while (temp->sledeci != NULL){
			temp = temp->sledeci;
		}

		// dodaj novi na kraj
		temp->sledeci = (struct red*)malloc(sizeof(struct red));
		temp = temp->sledeci;
		if (temp == NULL){
			printf("Nije uspelo rezervisanje memorije\n");
			exit(0);
		}

		temp->talas = _krug;
		temp->x = x;
		temp->y = y;
		temp->sledeci = NULL;
		temp->predhodni = _ispitajRed;

	}
	return(_ispitajRed);
}

struct red *pomerinasledeciCvor(struct red *_ispitajRed, int _krug){
	struct red *temp;

	if (_ispitajRed == NULL){
		printf("red je prazan\n");
		return(NULL);
	}

	_ispitajRed = _ispitajRed->sledeci;
	return(_ispitajRed);
}

void NadjiPut(int x, int y, struct red *_ispitajRed){


	int krug = 0;

	_ispitajRed = dodajCvor(_ispitajRed, x, y, krug);



	do{

		x = _ispitajRed->x;
		y = _ispitajRed->y;
		krug = _ispitajRed->talas;



		////////////////////////////////////
		//proverava tekuceg clana reda
		///////////////////////////////////
		if (L[x][y] == '.' || L[x][y] == 'S') // dozvoljeno polje
		{
			char xxx = krug ;
			L[x][y] = xxx;
		}
		else if (L[x][y] == 'K') // kraj
		{
			break;
		}
		///////////////////////////////////
		
			//dodaj susede u red
			if (x >= 0 && x <= MAXLAVIRINT && y - 1 >= 0 && y - 1 <= MAXLAVIRINT && L[x][y - 1] != 'Z' && (L[x][y - 1] == '.' || L[x][y - 1] == 'K')) //levo susedno polje
			{
				_ispitajRed = dodajCvor(_ispitajRed, x, y - 1, krug + 1);
			}

			if (x >= 0 && x <= MAXLAVIRINT && y + 1 >= 0 && y + 1 <= MAXLAVIRINT && L[x][y + 1] != 'Z' && (L[x][y + 1] == '.' || L[x][y + 1] == 'K')) //desno susedno polje
			{
				_ispitajRed = dodajCvor(_ispitajRed, x, y + 1, krug + 1);
				//pamtiPut = dodajuRed(pamtiPut, x, y + 1, krug + 1);
			}

			if (x - 1 >= 0 && x - 1 <= MAXLAVIRINT && y >= 0 && y <= MAXLAVIRINT && L[x - 1][y] != 'Z' && (L[x - 1][y] == '.' || L[x-1][y] == 'K')) //susedno polje gore
			{
				_ispitajRed = dodajCvor(_ispitajRed, x - 1, y, krug + 1);
				//pamtiPut = dodajuRed(pamtiPut, x - 1, y, krug + 1);
			}

			if (x + 1 >= 0 && x + 1 <= MAXLAVIRINT && y >= 0 && y <= MAXLAVIRINT && L[x + 1][y] != 'Z' && L[x + 1][y] == '.') //susedno polje dole
			{
				_ispitajRed = dodajCvor(_ispitajRed, x + 1, y, krug + 1);
			}



		//pomeri na sledeci cvor
			_ispitajRed = pomerinasledeciCvor(_ispitajRed, krug);

		prikazilavirint();



	} while ((_ispitajRed) != NULL);



	//kreiraj put
	struct red *temp;

	temp = _ispitajRed;
	ukTalas = _ispitajRed->talas;

	int i;
	a = (int *)malloc(sizeof(int )*temp->talas);
	b = (int *)malloc(sizeof(int)*temp->talas);

	while (temp != NULL)
	{
		a[temp->talas] = temp->x;
		b[temp->talas] = temp->y;
		temp = temp->predhodni;
	}
}

void pisiUFajl()
{	
	int i;
	FILE *lavirint;
	lavirint = fopen("lavirint.txt", "w");

	for (i = 1; i < ukTalas; i++)
	{
		fprintf(lavirint, "[%d,%d]", a[i],b[i]);
	}
	fclose(lavirint);
}

void pisiUFajlTop()
{
	int i;
	FILE *lavirintTop;
	lavirintTop = fopen("lavirintTop.txt", "w");

	for (i = 1; i < ukTalas; i++)
	{
		if (a[i] == a[i - 1] && a[i] == a[i + 1])
		{
			continue;
		}
		else 
		{
			if (b[i] == b[i - 1] && b[i] == b[i + 1])
			{
				continue;
			}	
			else
			{
				fprintf(lavirintTop, "[%d,%d]", a[i], b[i]);
			}	
		}		
	}
	fclose(lavirintTop);
}

int _tmain(int argc, _TCHAR* argv[])
{
	struct red *ispitajRed = NULL;

	NadjiPut(0, 11, ispitajRed);
	pisiUFajl();
	pisiUFajlTop();

	return 0;
}

