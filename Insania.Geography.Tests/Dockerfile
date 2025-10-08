#Использование образа sdk 9 dotnet
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

#Создание переменной конфигурации сборки
ARG BUILD_CONFIGURATION=Release

#Задание рабочего каталога
WORKDIR /src

#Копирование файлов в контейнер
COPY [".", ""]

#Восстановление проекта тестов
RUN dotnet restore "./Insania.Geography.Tests/Insania.Geography.Tests.csproj"

#Задание рабочего каталога
WORKDIR /src/Insania.Geography.Tests

#Сборка проекта тестов
RUN dotnet build "./Insania.Geography.Tests.csproj" -c $BUILD_CONFIGURATION -o /app/build

#Запуск тестов
RUN dotnet test "./Insania.Geography.Tests.csproj" -c $BUILD_CONFIGURATION --logger:"console;verbosity=normal"