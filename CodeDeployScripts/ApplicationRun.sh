cd /home/ec2-user
sudo chmod -R 700 ImageColourSwap/
sudo chown -R ec2-user ImageColourSwap/
cd /home/ec2-user/ImageColourSwap

export PATH=/home/ec2-user/.dotnet:$PATH
export DOTNET_CLI_HOME=/tmp/

cp ImageColourSwap.service /etc/systemd/system/
sudo systemctl start ImageColourSwap.service

cp ImageColourSwap.conf /etc/nginx/conf.d
sudo systemctl restart nginx

if [ -f '/etc/letsencrypt/ics.integration.michaelkillingbeck.com/fullchain.pem'];
then
    echo 'certificate exits'
else
    echo 'certificate does not exist'
    sudo certbot --nginx -d ics.integration.michaelkillingbeck.com --non-interactive --agree-tos -m michael@michaelkillingbeck.co.uk
fi