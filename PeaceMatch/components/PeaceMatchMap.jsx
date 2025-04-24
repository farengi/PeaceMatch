// PeaceMatchMap.jsx
import React, { useState, useEffect } from 'react';
import { GoogleMap, LoadScript, Marker, InfoWindow } from '@react-google-maps/api';
import './PeaceMatchMap.css';

// Sample data structure based on your C# model
const sampleData = {
  countries: [
    {
      name: "Lebanon",
      center: { lat: 33.8547, lng: 35.8623 },
      cities: [
        {
          name: "Beirut",
          position: { lat: 33.8938, lng: 35.5018 },
          resources: {
            food: 45.2,
            water: 38.7,
            clothes: 62.1,
            shelter: 55.0,
            warmth: 42.3,
            sleepEssentials: 50.8,
            sanitaryProducts: 35.6,
            femaleSanitaryProducts: 28.4
          }
        },
        {
          name: "Tripoli",
          position: { lat: 34.4333, lng: 35.8333 },
          resources: {
            food: 22.8,
            water: 25.3,
            clothes: 18.9,
            shelter: 20.0,
            warmth: 28.7,
            sleepEssentials: 19.2,
            sanitaryProducts: 24.4,
            femaleSanitaryProducts: 21.6
          }
        }
      ]
    },
    {
      name: "Syria",
      center: { lat: 35.0178, lng: 38.5078 },
      cities: [
        {
          name: "Damascus",
          position: { lat: 33.5138, lng: 36.2765 },
          resources: {
            food: 35.2,
            water: 28.7,
            clothes: 32.1,
            shelter: 25.0,
            warmth: 32.3,
            sleepEssentials: 30.8,
            sanitaryProducts: 25.6,
            femaleSanitaryProducts: 18.4
          }
        },
        {
          name: "Aleppo",
          position: { lat: 36.2021, lng: 37.1343 },
          resources: {
            food: 18.8,
            water: 15.3,
            clothes: 12.9,
            shelter: 10.0,
            warmth: 18.7,
            sleepEssentials: 14.2,
            sanitaryProducts: 16.4,
            femaleSanitaryProducts: 11.6
          }
        }
      ]
    }
  ]
};

// Function to determine status color based on resource levels
const getResourceColor = (percentage) => {
  if (percentage > 50) return '#10b981'; // green
  if (percentage > 30) return '#f59e0b'; // yellow
  return '#ef4444'; // red
};

// Function to calculate average resource percentage
const calculateAvgResourcePercentage = (resources) => {
  const values = Object.values(resources);
  return values.reduce((sum, val) => sum + val, 0) / values.length;
};

// Resource progress bar component
const ResourceBar = ({ label, percentage }) => (
  <div className="pm-resource-bar">
    <div className="pm-resource-header">
      <span className="pm-resource-label">{label}</span>
      <span className="pm-resource-label">{percentage.toFixed(1)}%</span>
    </div>
    <div className="pm-resource-track">
      <div 
        className={`pm-resource-progress ${percentage > 50 ? 'pm-badge-green' : percentage > 30 ? 'pm-badge-yellow' : 'pm-badge-red'}`}
        style={{ width: `${percentage}%` }}>
      </div>
    </div>
  </div>
);

const PeaceMatchMap = () => {
  const [selectedCountry, setSelectedCountry] = useState(null);
  const [selectedCity, setSelectedCity] = useState(null);
  const [mapData, setMapData] = useState(sampleData);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);
  const [showInfoWindow, setShowInfoWindow] = useState(false);
  
  // Map center and zoom level
  const [center, setCenter] = useState({ lat: 35.0, lng: 36.5 });
  const [zoom, setZoom] = useState(7);

  // Google Maps container style
  const containerStyle = {
    width: '100%',
    height: '100%'
  };

  // In a real application, you would fetch this data from your backend
  useEffect(() => {
    // Simulate data loading
    setIsLoading(true);
    // For demonstration, we'll just use the sample data
    setTimeout(() => {
      setMapData(sampleData);
      setIsLoading(false);
    }, 1000);
  }, []);

  const handleCountryClick = (country) => {
    setSelectedCountry(country === selectedCountry ? null : country);
    setSelectedCity(null);
    
    if (country && country !== selectedCountry) {
      // Center the map on the selected country
      setCenter(country.center);
      setZoom(8);
    } else {
      // Reset to default view
      setCenter({ lat: 35.0, lng: 36.5 });
      setZoom(7);
    }
  };

  const handleCityClick = (city) => {
    setSelectedCity(city === selectedCity ? null : city);
    setShowInfoWindow(true);
    
    // Find and set the country that contains this city
    const country = mapData.countries.find(
      c => c.cities.some(cityItem => cityItem.name === city.name)
    );
    
    if (country) {
      setSelectedCountry(country);
    }
    
    // Center the map on the selected city
    if (city) {
      setCenter(city.position);
      setZoom(10);
    }
  };

  if (isLoading) return (
    <div className="pm-loading">
      <div style={{ textAlign: 'center' }}>
        <div className="pm-loading-spinner"></div>
        <p>Loading resource distribution data...</p>
      </div>
    </div>
  );
  
  if (error) return (
    <div className="pm-error">
      <h4 className="pm-error-title">Error</h4>
      <p>{error}</p>
    </div>
  );

  return (
    <div className="pm-container">
      <div className="pm-header">
        <h1 className="pm-title">PeaceMatch Resource Distribution</h1>
        <div className="pm-legend-horizontal">
          <div className="pm-legend-item">
            <span className="pm-legend-dot" style={{ backgroundColor: '#10b981' }}></span>
            <span>Good (&gt;50%)</span>
          </div>
          <div className="pm-legend-item">
            <span className="pm-legend-dot" style={{ backgroundColor: '#f59e0b' }}></span>
            <span>Moderate (30-50%)</span>
          </div>
          <div className="pm-legend-item">
            <span className="pm-legend-dot" style={{ backgroundColor: '#ef4444' }}></span>
            <span>Critical (&lt;30%)</span>
          </div>
        </div>
      </div>
      
      <div className="pm-main">
        {/* Map View */}
        <div className="pm-map-container">
          <LoadScript googleMapsApiKey="AIzaSyDm4GUFoFKa71UZhhZBsmdKvcXCgXrEdI0">
            <GoogleMap
              mapContainerStyle={containerStyle}
              center={center}
              zoom={zoom}
              options={{
                styles: [
                  {
                    featureType: "administrative.country",
                    elementType: "geometry.stroke",
                    stylers: [{ color: "#666666" }, { weight: 1.5 }]
                  },
                  {
                    featureType: "administrative.country",
                    elementType: "labels.text.fill",
                    stylers: [{ color: "#444444" }]
                  }
                ]
              }}
            >
              {/* City markers */}
              {mapData.countries.flatMap(country => 
                country.cities.map(city => {
                  const avgPercentage = calculateAvgResourcePercentage(city.resources);
                  const markerColor = getResourceColor(avgPercentage);
                  
                  return (
                    <Marker
                      key={`${country.name}-${city.name}`}
                      position={city.position}
                      onClick={() => handleCityClick(city)}
                      icon={{
                        path: window.google && window.google.maps ? window.google.maps.SymbolPath.CIRCLE : 0,
                        fillColor: markerColor,
                        fillOpacity: 0.9,
                        strokeWeight: 2,
                        strokeColor: '#ffffff',
                        scale: 8,
                      }}
                    >
                      {selectedCity && selectedCity.name === city.name && showInfoWindow && (
                        <InfoWindow
                          position={city.position}
                          onCloseClick={() => setShowInfoWindow(false)}
                        >
                          <div className="pm-info-window">
                            <h3 className="pm-info-window-title">{city.name}</h3>
                            <p className="pm-info-window-text">
                              Avg. Resource: <strong>{avgPercentage.toFixed(1)}%</strong>
                            </p>
                            <p className="pm-info-window-subtext">
                              Click for detailed information
                            </p>
                          </div>
                        </InfoWindow>
                      )}
                    </Marker>
                  );
                })
              )}
            </GoogleMap>
          </LoadScript>
        </div>
        
        {/* Detail panel */}
        <div className="pm-detail-panel">
          {selectedCountry ? (
            <div>
              <h2 className="pm-subtitle">
                {selectedCountry.name}
              </h2>
              
              {selectedCity ? (
                <div>
                  <h3 className="pm-city-title">
                    {selectedCity.name}
                  </h3>
                  
                  <div className="pm-resource-container">
                    <div className="pm-resource-header-main">Resource Distribution:</div>
                    <ResourceBar label="Food" percentage={selectedCity.resources.food} />
                    <ResourceBar label="Water" percentage={selectedCity.resources.water} />
                    <ResourceBar label="Clothes" percentage={selectedCity.resources.clothes} />
                    <ResourceBar label="Shelter" percentage={selectedCity.resources.shelter} />
                    <ResourceBar label="Warmth" percentage={selectedCity.resources.warmth} />
                    <ResourceBar label="Sleep Essentials" percentage={selectedCity.resources.sleepEssentials} />
                    <ResourceBar label="Sanitary Products" percentage={selectedCity.resources.sanitaryProducts} />
                    <ResourceBar label="Female Sanitary Products" percentage={selectedCity.resources.femaleSanitaryProducts} />
                  </div>
                  
                  <div className="pm-city-info">
                    <div>Coordinates: {selectedCity.position.lat.toFixed(4)}, {selectedCity.position.lng.toFixed(4)}</div>
                    <div className="pm-avg-resource">
                      Average Resource: 
                      <span className={`pm-badge ${
                        calculateAvgResourcePercentage(selectedCity.resources) > 50 ? 'pm-badge-green' : 
                        calculateAvgResourcePercentage(selectedCity.resources) > 30 ? 'pm-badge-yellow' : 'pm-badge-red'
                      }`}>
                        {calculateAvgResourcePercentage(selectedCity.resources).toFixed(1)}%
                      </span>
                    </div>
                  </div>
                  
                  <button 
                    className="pm-button"
                    onClick={() => setSelectedCity(null)}
                  >
                    Back to {selectedCountry.name}
                  </button>
                </div>
              ) : (
                <div>
                  <p className="pm-instruction">Select a city from the map to view detailed resource information.</p>
                  
                  {selectedCountry.cities.map(city => {
                    const avgPercentage = calculateAvgResourcePercentage(city.resources);
                    return (
                      <div 
                        key={city.name}
                        className="pm-city-item"
                        onClick={() => handleCityClick(city)}
                      >
                        <div className="pm-city-item-header">
                          <span className="pm-city-item-name">{city.name}</span>
                          <span className={`pm-badge ${
                            avgPercentage > 50 ? 'pm-badge-green' : 
                            avgPercentage > 30 ? 'pm-badge-yellow' : 'pm-badge-red'
                          }`}>
                            {avgPercentage.toFixed(1)}%
                          </span>
                        </div>
                      </div>
                    );
                  })}
                  
                  <button 
                    className="pm-button"
                    onClick={() => handleCountryClick(null)}
                  >
                    Back to all countries
                  </button>
                </div>
              )}
            </div>
          ) : (
            <div>
              <h2 className="pm-subtitle">
                Countries
              </h2>
              <p className="pm-instruction">
                Select a country or city on the map to view resource distribution details.
              </p>
              
              {mapData.countries.map(country => {
                const countryAvg = country.cities.reduce((sum, city) => 
                  sum + calculateAvgResourcePercentage(city.resources), 0
                ) / country.cities.length;
                
                return (
                  <div 
                    key={country.name}
                    className="pm-country-item"
                    onClick={() => handleCountryClick(country)}
                  >
                    <div className="pm-country-item-header">
                      <span className="pm-country-item-name">{country.name}</span>
                      <span className={`pm-badge ${
                        countryAvg > 50 ? 'pm-badge-green' : 
                        countryAvg > 30 ? 'pm-badge-yellow' : 'pm-badge-red'
                      }`}>
                        {countryAvg.toFixed(1)}%
                      </span>
                    </div>
                    <div className="pm-country-item-cities">
                      {country.cities.length} cities
                    </div>
                  </div>
                );
              })}
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default PeaceMatchMap;