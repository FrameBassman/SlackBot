FROM microsoft/dotnet:latest

COPY . /app

WORKDIR /app/SetWalletBot.Presentation.Slack

RUN ["dotnet", "restore"]

RUN ["dotnet", "build"]

ENTRYPOINT ["dotnet", "run"]

CMD ["-c", "Release"]
