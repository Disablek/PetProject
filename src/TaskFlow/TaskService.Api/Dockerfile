# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Эти аргументы позволяют переключить базу, используемую для создания конечного образа при отладке из VS
ARG LAUNCHING_FROM_VS
# Это задает базовый образ для окончательной версии, но определен только LAUNCHING_FROM_VS
ARG FINAL_BASE_IMAGE=${LAUNCHING_FROM_VS:+aotdebug}

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# Установите зависимости clang/zlib1g-dev для публикации в машинном коде
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
    clang zlib1g-dev
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TaskService.Api/TaskService.Api.csproj", "TaskService.Api/"]
RUN dotnet restore "./TaskService.Api/TaskService.Api.csproj"
COPY . .
WORKDIR "/src/TaskService.Api"
RUN dotnet build "./TaskService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TaskService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=true

# Этот этап используется в качестве базового для последнего этапа при запуске из VS для поддержки отладки в обычном режиме (по умолчанию, если конфигурация отладки не используется)
FROM base AS aotdebug
USER root
# Установите GDB для поддержки собственной отладки
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
    gdb
USER app

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM ${FINAL_BASE_IMAGE:-mcr.microsoft.com/dotnet/runtime-deps:8.0} AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["./TaskService.Api"]