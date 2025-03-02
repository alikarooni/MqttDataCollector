import React, { useState, useEffect }  from 'react';
import Textbox from '../../Components/Textbox/Textbox';
import Dropdown from '../../Components/Dropdown/Dropdown';
import Button from '../../Components/Button/Button';
import { useConnection } from '../../Components/ConnectionProvider/ConnectionProvider';
import './AddProject.css';

const AddProject = () => {
  const dropdownOptions = [
    { value: '', label: 'Select an option' },
    { value: '0', label: 'Select 1' },
  ];

  const [domains, setDomains] = useState(dropdownOptions)
  const [domainId, setDomainId] = useState('')
  const [projectName, setProjectName] = useState('')
  const [brokerUrl, setBrokerUrl] = useState('')
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [port, setPort] = useState('')
  const [useTls, setUseTls] = useState(true)
  const [brokerTopic, setBrokerTopic] = useState('')
  const [eventGridTopic, setEventGridTopic] = useState('')

  const { connection, isConnected } = useConnection();

  useEffect(() => {
    if (isConnected && connection) {
        // Call any function or handle the connection here        
        connection.on('GetDomains', (domains) => {          
          setDomains(domains.map(domain => 
              ({ value: domain.id, label: domain.domainName })
            ))
        })        
        connection.invoke("GetDomains")
    }
  }, [connection, isConnected]); 

  const handleSubmit = () => {
    // Handle form submission logic here
    if (connection) {
      connection.on('AddProject', (message) => 
      { 
        console.log(message);
        setDomainId(domains[0].value)
        setProjectName('')
        setBrokerUrl('')
        setUsername('')
        setPassword('')
        setPort('')
        setUseTls(true)
        setBrokerTopic('')
        setEventGridTopic('')
      });
      connection.invoke("AddProject", domainId, projectName, brokerUrl,
      username, password, port, useTls, brokerTopic, eventGridTopic);
    }
  };

  const useTlsOptions = [
    { value: true, label: 'True' },
    { value: false, label: 'False' }
  ];

  useEffect(() => {
    setDomainId(domains[0].value)
  }, [domains, domainId]);

  return (    
    <form className="form-container">
      <h2>Add a new Domain</h2>
      <Dropdown
        options={domains}
        name="domainId"
        value={domainId}
        onChange={(e) => setDomainId(e.target.value)}  
        />
      <Textbox
        placeholder="Project Name"
        name="ProjectName"
        value={projectName}
        onChange={(e) => setProjectName(e.target.value)}
      />
      <Textbox
        placeholder="Broker Url"
        name="BrokerUrl"
        value={brokerUrl}
        onChange={(e) => setBrokerUrl(e.target.value)}
      />
      <Textbox
        placeholder="Username"
        name="Username"
        value={username}
        onChange={(e) => setUsername(e.target.value)}
      />
      <Textbox
        placeholder="Password"
        name="Password"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
      />
      <Textbox
        placeholder="Port"
        name="Port"
        value={port}
        onChange={(e) => setPort(e.target.value)}
      />
      <Dropdown
        options={useTlsOptions}
        name="useTls"
        value={useTls}
        onChange={(e) => setUseTls(e.target.value)}
        />
      <Textbox
        placeholder="Broker Topic"
        name="BrokerTopic"
        value={brokerTopic}
        onChange={(e) => setBrokerTopic(e.target.value)}
      />
      <Textbox
        placeholder="Event Grid Topic"
        name="EventGridTopic"
        value={eventGridTopic}
        onChange={(e) => setEventGridTopic(e.target.value)}
      />
      <Button label="Submit" onClick={handleSubmit} />
    </form>
  );
};

export default AddProject;
