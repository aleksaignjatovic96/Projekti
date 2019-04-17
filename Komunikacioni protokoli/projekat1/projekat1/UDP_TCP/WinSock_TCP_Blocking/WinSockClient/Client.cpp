#pragma comment(lib, "WS2_32.lib")
#pragma warning(disable:4996)
#define WIN32_LEAN_AND_MEAN

#include <windows.h>
#include <winsock2.h>
#include <ws2tcpip.h>
#include <stdlib.h>
#include <stdio.h>
#include <conio.h>
#include "../Biblioteka/Funkcije.h"
#include "../Biblioteka/StudentLista.h"

#define DEFAULT_BUFLEN 1024*1024*100
#define DEFAULT_PORT 27016


//192.168.100.210 DRUGI RACUNAR

StudentLista *glava = NULL;


// Initializes WinSock2 library
// Returns true if succeeded, false otherwise.
bool InitializeWindowsSockets();


int __cdecl main(int argc, char **argv)
{
	// socket used to communicate with server
	SOCKET connectSocket = INVALID_SOCKET;
	// variable used to store function return value
	int iResult;
	int iResultPekidz;



	// Validate the parameters
	if (argc != 2)
	{
		printf("usage: %s server-name\n", argv[0]);
		return 1;
	}

	if (InitializeWindowsSockets() == false)
	{
		// we won't log anything since it will be logged
		// by InitializeWindowsSockets() function
		return 1;
	}

	// create a socket
	connectSocket = socket(AF_INET,
		SOCK_STREAM,
		IPPROTO_TCP);

	if (connectSocket == INVALID_SOCKET)
	{
		printf("socket failed with error: %ld\n", WSAGetLastError());
		WSACleanup();
		return 1;
	}

	// create and initialize address structure
	sockaddr_in serverAddress;
	serverAddress.sin_family = AF_INET;
	serverAddress.sin_addr.s_addr = inet_addr(argv[1]);
	serverAddress.sin_port = htons(DEFAULT_PORT);
	// connect to server specified in serverAddress and socket connectSocket
	if (connect(connectSocket, (SOCKADDR*)&serverAddress, sizeof(serverAddress)) == SOCKET_ERROR)
	{
		printf("Unable to connect to server.\n");
		closesocket(connectSocket);
		WSACleanup();
	}

	unsigned long int nonBlockingMode = 1;
	iResult = ioctlsocket(connectSocket, FIONBIO, &nonBlockingMode);


	int size = DEFAULT_BUFLEN;

	//-------------------------------------------------------------
	//niti



	KriticnaSekcija* ks = inicijalizacija();
	DWORD dodajID, izbrisiID;
	HANDLE hNitDodaj, hNitIzbrisi;

	Student student1;
	student1.id = 0;
	student1.ime = "Nikola";
	student1.prezime = "Rebic";
	Student student2;
	student2.id = 1;
	student2.ime = "Aleksa";
	student2.prezime = "Ignjatovic";


	dodajUSredinuNit(&ks, student1);
	dodajUSredinuNit(&ks, student2);

	hNitDodaj = CreateThread(NULL, 0, &nitDodaj, ks, 0, &dodajID);
	hNitIzbrisi = CreateThread(NULL, 0, &nitIbrisi, ks, 0, &izbrisiID);



	//---------------------------------------------------------------------


	for (int j = 10; j < 100; j++)
	{
		Student stud;
		stud.id = j;
		stud.ime = "sas";
		stud.prezime = "asd";
		dodajNaPocetak(&glava, stud);
	}

	/*
	Student student1;
	student1.id = 1;
	student1.ime = "Nikola";
	student1.prezime = "Rebic";
	Student student2;
	student2.id = 2;
	student2.ime = "Aleksa";
	student2.prezime = "Ignjatovic";
	Student student4;
	student4.id = 4;
	student4.ime = "Igor";
	student4.prezime = "Karadzic";
	Student student3;
	student3.id = 3;
	student3.ime = "Itan";
	student3.prezime = "Kurtesi";
	Student student33;
	student33.id = 3;
	student33.ime = "Igor";
	student33.prezime = "Kuzmanovic";

	dodajUSredinu(&glava, student1);
	ispisiListu(&glava);
	dodajUSredinu(&glava, student2);
	ispisiListu(&glava);
	dodajUSredinu(&glava, student4);
	ispisiListu(&glava);
	dodajUSredinu(&glava, student3);
	ispisiListu(&glava);
	dodajUSredinu(&glava, student33);
	ispisiListu(&glava);
	//izbrisiStudenta(&glava, student2.id);
	//ispisiListu(&glava); */

	int duzina = racunajDuzinuListe(&glava);
	int *niz = racunajVelicinuPoljaUStudentu(&glava);

	int duzinaNiza = vratiBrojBajtovaUNizu(&glava)*sizeof(int);
	printf("Duzina liste je %d bajtova\n", duzina);
	printf("Duzina niza je %d bajtova\n", duzinaNiza);
	// message to send
	//char *messageToSend = (char*)malloc(size); 100MB

	int ukupnaDuzina = duzina + duzinaNiza + 4;

	//STUDENTI//
	char *messageToSend = (char*)malloc(ukupnaDuzina);


	serijalizacija(&glava, messageToSend, niz, duzina, duzinaNiza);


	do
	{

			// Send an prepared message with null terminator included
			//iResult = SendSafe(connectSocket, (char*)&size, sizeof(size));   100MB


			//STUDENTI//
			iResult = SendSafe(connectSocket, (char*)&ukupnaDuzina, sizeof(ukupnaDuzina));

			printf("Bytes Sent: %ld\n", iResult);


			//iResultPekidz = SendSafe(connectSocket, messageToSend, size); 100MB

			//STUDENTI//
			iResultPekidz = SendSafe(connectSocket, messageToSend, ukupnaDuzina);
		

	} while (iResultPekidz != ukupnaDuzina);

	printf("Bytes Sent: %ld\n", iResultPekidz);

	// cleanup
	system("PAUSE");
	CloseHandle(hNitDodaj);
	CloseHandle(hNitIzbrisi);
	closesocket(connectSocket);
	WSACleanup();

	return 0;
}

bool InitializeWindowsSockets()
{
	WSADATA wsaData;
	// Initialize windows sockets library for this process
	if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0)
	{
		printf("WSAStartup failed with error: %d\n", WSAGetLastError());
		return false;
	}
	return true;
}

