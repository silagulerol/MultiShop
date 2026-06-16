#!/bin/bash

ROOT_DIR="/Users/silagulerol/Desktop/MultiShop"

osascript <<EOF
tell application "Terminal"
    do script "cd $ROOT_DIR/IdentityServer/MultiShop.IdentityServer && dotnet run"
    do script "cd $ROOT_DIR/Services/Catalog/MultiShop.Catalog && dotnet run"
    do script "cd $ROOT_DIR/ApiGateway/MultiShop.OcelotApiGateway && dotnet run"
    do script "cd $ROOT_DIR/Frontends/MultiShop.WebUI && dotnet run --launch-profile https'"
end tell
EOF
