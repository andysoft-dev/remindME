#!/bin/bash


echo "Instalador de remindME"
cp remindme.sh /etc/profile.d/remindme.sh

tar -xf remindME-Linux-v.0.0.1.tar.gz -C /usr/local/bin/remindME

cd /usr/local/bin/remindME

chmod +x remindME
