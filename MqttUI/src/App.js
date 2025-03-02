import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Header from './Components/Header/Header';
import Sidebar from './Components/Sidebar/Sidebar';
import LiveView from './Pages/LiveView/LiveView';
import AddProject from './Pages/AddProject/AddProject';
import AddDomain from './Pages/AddDomain/AddDomain';
import { ConnectionProvider } from './Components/ConnectionProvider/ConnectionProvider';
import './App.css';

function App() {

  return (
    <Router>
      <ConnectionProvider>
        <div className="app">
          <Header />
          <Sidebar />
          <main className="main-content">
            <Routes>
                <Route path="/" element={<LiveView />} />
                <Route path="/adddomain" element={<AddDomain />} />
                <Route path="/addproject" element={<AddProject />} />
            </Routes>            
          </main>
        </div>
      </ConnectionProvider>
    </Router>
  );
}

export default App;