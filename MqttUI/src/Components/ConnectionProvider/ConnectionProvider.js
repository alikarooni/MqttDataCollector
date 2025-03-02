import React, { createContext, useContext, useEffect, useState } from 'react';
import * as signalR from '@microsoft/signalr';

// Create a context
export const ConnectionContext = createContext();

// Provider component that wraps your app and makes the connection object available to any child component
export const ConnectionProvider = ({ children }) => {
    const [connection, setConnection] = useState(null);
    const [isConnected, setIsConnected] = useState(false);

    useEffect(() => {
        // Initialize the SignalR connection
        const newConnection = new signalR.HubConnectionBuilder()
            .withUrl('https://mqttconsumerwebapp.azurewebsites.net/signalrservice')
            // .configureLogging(signalR.LogLevel.Information)
            .withAutomaticReconnect()
            .build();

        // Start the connection
        const establishConnection = async () => {
            try {
                await newConnection.start();
                console.log('Connection started!');
                setIsConnected(true); // Update connection status
                setConnection(newConnection);
            } catch (err) {
                console.error('Error while establishing connection:', err);
                setIsConnected(false);
                setConnection(newConnection);
            }
        };
        establishConnection();
        return () => {
            newConnection.stop().then(() => console.log('Connection stopped.'));
        }
    }, []);

    return (
        <ConnectionContext.Provider value={{connection, isConnected }}>
            {children}
        </ConnectionContext.Provider>
    );
}

// Custom hook to use the connection
export const useConnection = () => useContext(ConnectionContext);
