service_exists() {
    if [[ $(systemctl list-units --all -t service --full --no-legend "$1.service" | sed 's/^\s*//g' | cut -f1 -d' ') == $1.service ]]; then
        return 0
    else
        return 1
    fi
}
if service_exists ImageColourSwap; then
    sudo systemctl stop ImageColourSwap.service
fi

output=$(curl -s https://checkip.amazonaws.com)
hostedZoneID=$(aws route53 list-hosted-zones-by-name |  jq --arg name "michaelkillingbeck.com." -r '.HostedZones | .[] | select(.Name=="\($name)") | .Id')

echo $(jq --arg output "$output" '.Changes[0].ResourceRecordSet.ResourceRecords[0].Value = $output' hosted-zone.json) > hosted-zone.json
aws route53 change-resource-record-sets --hosted-zone-id $hostedZoneID --change-batch file://hosted-zone.json