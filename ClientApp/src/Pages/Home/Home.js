import React, { Component, useState, useEffect } from 'react';

const Home=(props)=> {
  const [posts, setPosts] = useState([]);

    useEffect(() => {
        (async function () {
           
        })()
    }, []);

    return (
        <div>
            <h1>Posts App</h1>
            <div>
               <p>Hello, welcome to my WEB App for Azure exercises :D </p>
            </div>
        </div>
    )

}

export default Home;
