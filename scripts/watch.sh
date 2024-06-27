#!/bin/bash

if [ -z "$CelestePrefix" ]; then
	echo -e "\033[31m----! \$CelestePrefix not found"
	exit 1
fi

if ! which inotifywatch 2>&1 > /dev/null; then
	echo -e "\033[31m----! `inotifywatch` not found"
	exit 1
fi

mod_name=`pwd`
mod_name=${mod_name##*/}
mod_dir=$CelestePrefix/Mods/${mod_name}
echo -e "\033[34m----i Mod ${mod_name} @ ${mod_dir}\033[0m"

echo -e "\033[32m----> Watching\033[0m"
inotifywait -m -r -e 'modify,create,move' --exclude '\./bin|\./obj' . | 
    while read file_path file_event file_name; do 
        echo -e "\033[34m----i Event ${file_path}${file_name} [${file_event}]\033[0m"

        if [[ "${file_name}" == *.cs ]]; then
            ./scripts/build.sh
            cp ./bin/${mod_name}.dll ${mod_dir}/bin/${mod_name}.dll
            continue
        fi
        
        if [[ "${file_name}" == *\~ ]]; then
            continue
        elif [[ "${file_name}" == *.bin ]]; then
            echo -e "\033[32m----> Copying Map\033[0m"
        elif [[ "${file_path}" == ./Loenn/* ]]; then
            echo -e "\033[32m----> Copying Loenn\033[0m"
        elif [[ "${file_path}" == ./Dialog/* ]]; then
            echo -e "\033[32m----> Copying Dialog\033[0m"
        else
            continue
        fi

        if [ -d ${mod_dir} ]; then
            rm -f ${mod_dir}${file_path:1}${file_name}
            cp ${file_path}${file_name} ${mod_dir}${file_path:1}
        else
            echo -e "\033[32m----> Copying All\033[0m"
            cp -r . ${mod_dir}
        fi
    done
