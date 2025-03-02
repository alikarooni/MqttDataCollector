import { Link } from 'react-router-dom';
import './Sidebar.css';

const Sidebar = () => {
  return (
    <aside className="sidebar">
      <ul>
        <li><Link to="/" >Live Data</Link></li>
        <li><Link to="/adddomain">Add Domain</Link></li>
        <li><Link to="/addproject" >Add Project</Link></li>        
      </ul>
    </aside>
  );
};

export default Sidebar;