#include <ws2tcpip.h>
#include <stdlib.h>
#include <stdio.h>
#include "../Biblioteka/Funkcije.h"
#include "../Biblioteka/StudentLista.h"

#define DEFAULT_BUFLEN 1024*1024*100
#define DEFAULT_PORT "27016"



bool InitializeWindowsSockets();
StudentLista *glava = NULL;


int  main(void)
{
	// Socket used for listening for new clients 
	SOCKET listenSocket = INVALID_SOCKET;
	// Socket used for communication with client
	SOCKET acceptedSocket = INVALID_SOCKET;
	// variable used to store function return value
	int iResult;
	int size = DEFAULT_BUFLEN;
	// message to recv
	char *messageToRecv = (char*)malloc(size);
	char *messageToRecvPekidz = NULL;

	if (InitializeWindowsSockets() == false)
	{
		// we won't log anything since it will be logged
		// by InitializeWindowsSockets() function
		return 1;
	}

	// Prepare address information structures
	addrinfo *resultingAddress = NULL;
	addrinfo hints;

	memset(&hints, 0, sizeof(hints));
	hints.ai_family = AF_INET;       // IPv4 address
	hints.ai_socktype = SOCK_STREAM; // Provide reliable data streaming
	hints.ai_protocol = IPPROTO_TCP; // Use TCP protocol
	hints.ai_flags = AI_PASSIVE;     // 

									 // Resolve the server address and port
	iResult = getaddrinfo(NULL, DEFAULT_PORT, &hints, &resultingAddress);
	if (iResult != 0)
	{
		printf("getaddrinfo failed with error: %d\n", iResult);
		WSACleanup();
		return 1;
	}

	// Create a SOCKET for connecting to server
	listenSocket = socket(AF_INET,      // IPv4 address famly
		SOCK_STREAM,  // stream socket
		IPPROTO_TCP); // TCP

	if (listenSocket == INVALID_SOCKET)
	{
		printf("socket failed with error: %ld\n", WSAGetLastError());
		freeaddrinfo(resultingAddress);
		WSACleanup();
		return 1;
	}

	// Setup the TCP listening socket - bind port number and local address 
	// to socket
	iResult = bind(listenSocket, resultingAddress->ai_addr, (int)resultingAddress->ai_addrlen);
	if (iResult == SOCKET_ERROR)
	{
		printf("bind failed with error: %d\n", WSAGetLastError());
		freeaddrinfo(resultingAddress);
		closesocket(listenSocket);
		WSACleanup();
		return 1;
	}

	// Since we don't need resultingAddress any more, free it
	freeaddrinfo(resultingAddress);

	// Set listenSocket in listening mode
	iResult = listen(listenSocket, SOMAXCONN);
	if (iResult == SOCKET_ERROR)
	{
		printf("listen failed with error: %d\n", WSAGetLastError());
		closesocket(listenSocket);
		WSACleanup();
		return 1;
	}

	unsigned long int nonBlockingMode = 1;

	iResult = ioctlsocket(listenSocket, FIONBIO, &nonBlockingMode);

	printf("Server initialized, waiting for clients.\n");

	

	do
	{
			FD_SET set;
			timeval timeVal;

			FD_ZERO(&set);
			// Add socket we will wait to read from
			FD_SET(listenSocket, &set);
			// Set timeouts to zero since we want select to return
			// instantaneously
			timeVal.tv_sec = 0;
			timeVal.tv_usec = 0;

			int iResult = select(0 /* ignored */, &set, NULL, NULL, &timeVal);

			


			if (iResult > 0)
			{
				acceptedSocket = accept(listenSocket, NULL, NULL);

				iResult = ioctlsocket(acceptedSocket, FIONBIO, &nonBlockingMode);

				if (acceptedSocket == INVALID_SOCKET)
				{

					printf("accept failed with error: %d\n", WSAGetLastError());
					closesocket(listenSocket);
					WSACleanup();
					return 1;


				}

				iResult = RecvSafe(acceptedSocket, messageToRecv, &messageToRecvPekidz);
				printf("Broj bajtova ukupno: %d\n", iResult);


				/*	Student student5;
				student5.id = 5;
				student5.ime = "Milan";
				student5.prezime = "Vujkovic";
				dodajUSredinu(&glava, student5); */

				deserijalizacija(&glava, messageToRecv, messageToRecvPekidz);
				ispisiListu(&glava);
				/*
				printf("-------------------------------------------------------------\n");

				Student student2;
				student2.id = 2;
				student2.ime = "Tata";
				student2.prezime = "Mata";

				dodajUSredinu(&glava, student2);
				ispisiListu(&glava);
				printf("-------------------------------------------------------------\n");

				Student student0;
				student0.id = 0;
				student0.ime = "Aleksandar";
				student0.prezime = "Jovanov";

				dodajUSredinu(&glava, student0);
				ispisiListu(&glava);

				izbrisiStudenta(&glava, 3);
				ispisiListu(&glava);
				*/
				// here is where server shutdown loguc could be placed

			}

	} while (1);

	// shutdown the connection since we're done
	iResult = shutdown(acceptedSocket, SD_SEND);
	if (iResult == SOCKET_ERROR)
	{
		printf("shutdown failed with error: %d\n", WSAGetLastError());
		closesocket(acceptedSocket);
		WSACleanup();
		return 1;
	}

	// cleanup
	closesocket(listenSocket);
	closesocket(acceptedSocket);
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

