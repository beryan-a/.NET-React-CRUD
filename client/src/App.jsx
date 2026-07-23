import { useState } from 'react'
import Navbar from './components/Navbar';
import { Routes, Route } from "react-router";
import { Home } from './pages/Home';
import { About } from './pages/About';
import { NotFound } from './pages/NotFound';
import { Person } from './components/person/Person';
import './App.css'

function App() {
  
  return (
    <>
      {/* Navigation */}
      <Navbar/>

      {/* Routes */}
      <Routes>
        <Route path="/" element={<Home/>}/>
        <Route path="/about" element={<About/>}/>
        <Route path="/Person" element={<Person/>}/>
        <Route path="" element={<NotFound/>}/>
      </Routes>

    </>
    
  )
}

export default App;
