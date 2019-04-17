#pragma once

#include <winsock2.h>
#include <stdio.h>

int SendSafe(SOCKET, char*, int);
int RecvSafe(SOCKET, char*, char**);
