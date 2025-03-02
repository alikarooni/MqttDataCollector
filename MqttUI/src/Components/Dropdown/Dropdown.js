import React from 'react';
import './Dropdown.css';

const Dropdown = ({ options, name, value, onChange }) => {
  return (
    <select className="myDropdown" name={name} value={value} onChange={onChange}>
      {options.map((option, index) => (
        <option key={index} value={option.value}>
          {option.label}
        </option>
      ))}
    </select>
  );
};

export default Dropdown;
