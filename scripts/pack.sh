#!/bin/bash

mod_name=`pwd`
mod_name=${mod_name##*/}

rm -f ${mod_name}.zip
zip ${mod_name}.zip -r LICENSE.txt everest.yaml Loenn Dialog Maps bin