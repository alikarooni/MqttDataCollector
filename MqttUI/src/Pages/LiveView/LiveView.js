import React, { useState, useEffect }  from 'react';
import { useConnection } from '../../Components/ConnectionProvider/ConnectionProvider'
import { Line } from 'react-chartjs-2';
import { Chart as ChartJS, CategoryScale, LinearScale, PointElement, LineElement, Title, Tooltip, Legend } from 'chart.js';
import './LiveView.css';

ChartJS.register(CategoryScale, LinearScale, PointElement, LineElement, Title, Tooltip, Legend);

const LiveView = () => {
    const defauleChartData = { 
        labels: [], 
        datasets: [] };
    const [chartData, setChartData] = useState(defauleChartData);

    
    
    function getRandomColor() {
        const red = Math.floor(Math.random() * 256);  
        const green = Math.floor(Math.random() * 256);
        const blue = Math.floor(Math.random() * 256); 
        return `rgb(${red}, ${green}, ${blue})`;
    }

    const { connection, isConnected } = useConnection();
    useEffect(() => {
        const addData = (eventTime, eventTopic, eventValue) => {
            setChartData(prevChartData => {
                const newLabels = [eventTime, ...prevChartData.labels].slice(0, 20);
                let newDatasets = [...prevChartData.datasets];
                const datasetIndex = newDatasets.findIndex(ds => ds.label === eventTopic);
        
                if (datasetIndex === -1) {
                    // Add new dataset if it does not exist
                    newDatasets = [{
                        label: eventTopic,
                        data: [eventValue],
                        borderColor: getRandomColor(),
                        tension: 0.1
                    }, ...newDatasets];
                } else {
                    // Update existing dataset
                    const newData = [eventValue, ...newDatasets[datasetIndex].data].slice(0, 20);
                    newDatasets[datasetIndex] = { ...newDatasets[datasetIndex], data: newData };
                }
                prevChartData.datasets.forEach((dataset, index) => {
                    if(index !== datasetIndex){
                        const len = dataset.data.length
                        dataset.data = [dataset.data[len-1], ...dataset.data].slice(0, 20)
                    }
                })
                return { labels: newLabels, datasets: newDatasets };
            });
        }
        
        if (isConnected && connection) {
            connection.on('ReciveMqttEvent', (projectId, projectName, brokerTopic, eventTopic, eventValue, eventTime) => {
                const datetime = new Date(eventTime)
                const newEventTime = `${datetime.getHours()}:${datetime.getMinutes()}:${datetime.getSeconds()}.${datetime.getMilliseconds()}`
                addData(newEventTime, eventTopic, parseInt(eventValue, 10))                
            })        
        }
    }, [connection, isConnected]); 

    return (
        <div className='LiveView'>
            <Line data={chartData} />
        </div>
    );
};

export default LiveView;