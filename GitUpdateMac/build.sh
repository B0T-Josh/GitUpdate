#!/bin/zsh

dotnet publish -c Release -r osx-arm64 --self-contained true /p:PublishSingleFile=true && mv bin/Release/net10.0/osx-arm64/publish/up /Users/josh/Documents/GitHub/GitUpdate/GitUpdateMac && cp up ~/.local/bin