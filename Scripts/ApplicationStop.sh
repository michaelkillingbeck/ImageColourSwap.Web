service_exists() {
    local serviceName=$1
    if [[ $(systemctl list-units --all -t service --full --no-legend "$serviceName.service" | sed 's/^\s*//g' | cut -f1 -d' ') == $n.service ]]; then
        return 0
    else
        return 1
    fi
}
if service_exists ImageColourSwap; then
    sudo systemctl stop ImageColourSwap.service
fi