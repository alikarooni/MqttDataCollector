# Project Suite: MQTT Data Collector

This repository combines three micro services—**MqttPublisher**, **MqttConsumer**, and **MqttUI**—into a unified suite for managing MQTT and event-driven workflows. Each lives in its own subdirectory, preserving its history and purpose.

## MqttPublisher
A C# app that publishes MQTT messages to Azure Event Grid, using MongoDB for configs.

### Key Components
- **MQTT Integration**: Sends messages to Azure Event Grid.
- **Data Storage**: MongoDB for project and domain setups.
- **Event Publishing**: Scales via Azure’s event system.

## MqttConsumer
An ASP.NET Core app that consumes MQTT events and pushes real-time updates via SignalR, backed by MongoDB.

### Key Components
- **Event Consumption**: Processes MQTT events.
- **Real-Time**: Delivers updates with SignalR.
- **Data Persistence**: Stores events in MongoDB.
- **Project & Domain Management**: Organizes event scopes.

## MqttUI
A React app (built with Create React App) for managing projects and domains tied to data streaming.

### Key Components
- **Project Management**: Configures broker settings.
- **Domain Handling**: Links domains to streaming systems.
- **Live Data**: Displays real-time feeds.

