#!/bin/bash

echo "Instalador de remindME"

cp remindme.sh /etc/profile.d/


tar -xf remindME-Linux.tar.gz -C /user/local/bin/remindME/
cd /user/local/bin/remindME
chmod -x remindME


