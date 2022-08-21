sudo /home/ec2-user

touch output.txt

sudo wget https://dot.net/v1/dotnet-install.sh >> output.txt
sudo chmod +x ./dotnet-install.sh >> output.txt
sudo ./dotnet-install.sh >> output.txt

cd /home/ec2-user/ImageColourSwap

export PATH=/home/ec2-user/.dotnet:$PATH
export DOTNET_CLI_HOME=/tmp/

cp ImageColourSwap.service /etc/systemd/system/
sudo systemctl start ImageColourSwap.service

cp ImageColourSwap.conf /etc/nginx/conf.d
sudo systemctl restart nginx