#!/bin/bash

if [ -z "$CelestePrefix" ]; then
	echo -e "\033[31m----! \$CelestePrefix not found"
	exit 1
fi

echo -e "\033[32m----> Building\033[0m"
if ! dotnet build; then
	echo -e "\033[31m----! Build failed"
	exit 1
fi