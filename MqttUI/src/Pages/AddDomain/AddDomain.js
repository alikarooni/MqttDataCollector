import React, { useState } from 'react';
import { useConnection } from '../../Components/ConnectionProvider/ConnectionProvider';
import Textbox from '../../Components/Textbox/Textbox';
import Button from '../../Components/Button/Button';
import './AddDomain.css';

const AddDomain = () => {
  const [domainName, setDomainName] = useState('')
  const [domainEndPoint, setDomainEndPoint] = useState('')
  const [domainSecretKey, setDomainSecretKey] = useState('')

  const { connection } = useConnection();

  const sendMessage = () => {
    if (connection) {
      connection.on('AddDomain', () => 
      { 
        console.log("domain added.");
        setDomainName('')
        setDomainEndPoint('')
        setDomainSecretKey('')
      });
      connection.invoke("AddDomain", domainName, domainEndPoint, domainSecretKey);
    }
  };

  const handleSubmit = () => {
    // Handle form submission logic here
    sendMessage();
  };

  return (    
    <form className="form-container">
      <h2>Add a new Domain</h2>
      <Textbox
        placeholder="Domain Name"
        name="domainName"
        value={domainName}
        onChange={(e) => setDomainName(e.value)}
      />
      <Textbox
        placeholder="Domain EndPoint"
        name="domainEndPoint"
        value={domainEndPoint}
        onChange={(e) => setDomainEndPoint(e.value)}
      />
      <Textbox
        placeholder="Domain Secret Key"
        name="domainSecretKey"
        value={domainSecretKey}
        onChange={(e) => setDomainSecretKey(e.value)}
      />
      <Button label="Submit" onClick={handleSubmit} />
    </form>
  );
};

export default AddDomain;
