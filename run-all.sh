#!/bin/bash

osascript -e 'tell application "Terminal" to do script "cd ~/Desktop/MultiShop/IdentityServer/MultiShop.IdentityServer && dotnet run"'
osascript -e 'tell application "Terminal" to do script "cd ~/Desktop/MultiShop/Services/Catalog/MultiShop.Catalog && dotnet run"'
osascript -e 'tell application "Terminal" to do script "cd ~/Desktop/MultiShop/ApiGateway/MultiShop.OcelotGateway && dotnet run"'
osascript -e 'tell application "Terminal" to do script "cd ~/Desktop/MultiShop/Frontends/MultiShop.WebUI && dotnet run --launch-profile https"'
osascript -e 'tell application "Terminal" to do script "cd ~/Desktop/MultiShop/Services/Discount/MultiShop.Discount && dotnet run"'
osascript -e 'tell application "Terminal" to do script "cd ~/Desktop/MultiShop/Services/Comment/MultiShop.Comment && dotnet run"'
osascript -e 'tell application "Terminal" to do script "cd ~/Desktop/MultiShop/Services/Order/Presentation/MultiShop.Order.WebApi && dotnet run"'
osascript -e 'tell application "Terminal" to do script "cd ~/Desktop/MultiShop/Services/Cargo/MultiShop.Cargo.WebApi && dotnet run"'
osascript -e 'tell application "Terminal" to do script "cd ~/Desktop/MultiShop/Services/Basket/MultiShop.Basket && dotnet run"'
osascript -e 'tell application "Terminal" to do script "cd ~/Desktop/MultiShop/Services/Message/MultiShop.Message && dotnet run"'
osascript -e 'tell application "Terminal" to do script "cd ~/Desktop/MultiShop/Services/Payment/MultiShop.Payment && dotnet run"'