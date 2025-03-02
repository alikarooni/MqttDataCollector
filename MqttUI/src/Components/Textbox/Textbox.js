import React from 'react';
import './Textbox.css';

const Textbox = ({ placeholder, name, value, onChange }) => {
  return (
    <input
      type="text"
      className="myTextBox"
      placeholder={placeholder}
      name={name}
      value={value}
      onChange={onChange}
    />
  );
};

export default Textbox;
