#include "Funkcije.h"

int SendSafe(SOCKET connectSocket, char* messageToSend, int length)
{
	int poslato = 0;

	//non-blocking 
	// Initialize select parameters
	FD_SET set;
	timeval timeVal;

	do
	{




		FD_ZERO(&set);
		// Add socket we will wait to read from
		FD_SET(connectSocket, &set);
		// Set timeouts to zero since we want select to return
		// instantaneously
		timeVal.tv_sec = 0;
		timeVal.tv_usec = 0;

		int iResult = select(0 /* ignored */, NULL, &set, NULL, &timeVal);

		if (iResult > 0)
		{
			// Send an prepared message with null terminator included
			iResult = send(connectSocket, messageToSend + poslato, length - poslato, 0);

			if (iResult == SOCKET_ERROR)
			{
				printf("send failed with error: %d\n", WSAGetLastError());
				closesocket(connectSocket);
				WSACleanup();
				return 1;
			}

			poslato += iResult;
		}
		

	} while (poslato != length);

	return poslato;
}

int RecvSafe(SOCKET acceptedSocket, char* messageToRecv, char** messageToRecvPekidz)
{
	int poslato = 0;
	int iResult = 0;

	//non-blocking 
	// Initialize select parameters
	FD_SET set;
	timeval timeVal;
	// Set timeouts to zero since we want select to return
	// instantaneously
	timeVal.tv_sec = 0;
	timeVal.tv_usec = 0;


	do
	{

		FD_ZERO(&set);
		// Add socket we will wait to read from
		FD_SET(acceptedSocket, &set);
		// Set timeouts to zero since we want select to return
		// instantaneously
		timeVal.tv_sec = 0;
		timeVal.tv_usec = 0;

		iResult = select(0 /* ignored */, &set, NULL, NULL, &timeVal);

		if (iResult > 0)
		{
			iResult = recv(acceptedSocket, messageToRecv + poslato, sizeof(messageToRecv) - poslato, 0);
			if (iResult > 0)
			{
				printf("Broj bajtova: %d\n", iResult);
			}
			else if (iResult == 0)
			{
				// connection was closed gracefully
				printf("Connection with client closed.\n");
				closesocket(acceptedSocket);
			}
			else
			{
				// there was an error during recv
				printf("recv failed with error: %d\n", WSAGetLastError());
				closesocket(acceptedSocket);
			}

			poslato += iResult;

		}

		else if (iResult < 0)
		{
			// there was an error during recv
			printf("Nema klijenta\n");
			closesocket(acceptedSocket);
		}

		

    } while (poslato != sizeof(messageToRecv));

	int length = *(int*)messageToRecv;

	*messageToRecvPekidz = (char*)malloc(length);

	poslato = 0;

	do
	{

		FD_ZERO(&set);
		// Add socket we will wait to read from
		FD_SET(acceptedSocket, &set);
		// Set timeouts to zero since we want select to return
		// instantaneously
		timeVal.tv_sec = 0;
		timeVal.tv_usec = 0;

		iResult = select(0 /* ignored */, &set, NULL, NULL, &timeVal);

		if (iResult > 0)
		{
			// Receive data until the client shuts down the connection
			iResult = recv(acceptedSocket, *messageToRecvPekidz + poslato, length - poslato, 0);
			if (iResult > 0)
			{
				printf("Broj bajtova: %d\n", iResult);
			}
			else if (iResult == 0)
			{
				// connection was closed gracefully
				printf("Connection with client closed.\n");
				closesocket(acceptedSocket);
			}
			else
			{
				// there was an error during recv
				printf("recv failed with error: %d\n", WSAGetLastError());
				closesocket(acceptedSocket);
			}


			poslato += iResult;

		}
		else if (iResult < 0)
		{
			// there was an error during recv
			printf("recv failed with error: %d\n", WSAGetLastError());
			closesocket(acceptedSocket);
		}

		

	} while (poslato != length);

	printf("Connection with client closed.\n");
	closesocket(acceptedSocket);

	return poslato;
}