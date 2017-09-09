FROM microsoft/dotnet:latest

COPY ./SetWalletBot.Presentation.Slack /app

WORKDIR /app

RUN ["dotnet", "restore"]

RUN ["dotnet", "build"]

ENTRYPOINT ["dotnet", "run"]

CMD ["-c", "Release"]
