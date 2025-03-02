import React from 'react';
import './Button.css';

const Button = ({ label, onClick }) => {
  return (
    <button type="button" 
      className="myButton"
      onClick={onClick}>
      {label}
    </button>
  );
};

export default Button;
