#!/bin/bash

echo "Instalador de remindME"

cd /etc/profile.d/

printf "#!/bin/bash\nexport PATH=\$PATH:/usr/local/bin/remindME" > reminder.sh

tar -xf remindME-Linux.tar.gz -C /user/local/bin/remindME/
cd /user/local/bin/remindME
chmod -x remindME


