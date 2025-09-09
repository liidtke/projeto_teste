import React, { useState, useEffect } from 'react';
import axios from 'axios';

const Triages = () => {
  // State for triage records
  const [triages, setTriages] = useState([]);
  // State for loading and error messages
  const [loading, setLoading] = useState(false);
  const [apiError, setApiError] = useState('');
   function handleTriageClick(){

  }
  // Fetch triage records on component mount
  useEffect(() => {
    const fetchTriages = async () => {
      setLoading(true);
      try {
        const response = await axios.get('http://localhost:5000/api/triages');
        setTriages(response.data);
      } catch (error) {
        console.error('Erro ao buscar', error);
        setApiError('Erro ao buscar');
      } finally {
        setLoading(false);
      }
    };
    fetchTriages();
  }, []);


  return (
    <div className="max-w-4xl mx-auto p-6 bg-base-100 shadow-xl rounded-lg">
      <h2 className="text-2xl font-bold mb-6 text-center">Triagem</h2>
  <div className='mb-4'>
        <a href='/atendimento/' className='btn btn-default'>Atendimento</a>
        <a href='/pacientes' className='btn btn-ghost'>Criar Paciente</a>
      </div>
      <div className='divider mb-4'></div>



      {loading && <p className="text-info">Loading triage records...</p>}
      {apiError && <p className="text-error">{apiError}</p>}
      {triages.length === 0 && !loading && !apiError && (
        <p className="text-warning">No triage records found.</p>
      )}

      {triages.length > 0 && (
        <div className="overflow-x-auto">
          <table className="table w-full">
            <thead>
              <tr>
                <th>Paciente</th>
                <th>Data</th>
                <th>Sintomas</th>
                <th>Pressão Arterial</th>
                <th>Peso (kg)</th>
                <th>Altura (m)</th>
                {/* <th>Kind </th> */}
              </tr>
            </thead>
            <tbody>
              {triages.map((triage) => (
                <tr
                  key={triage.id}
                  className="hover cursor-pointer"
                  onClick={() => handleTriageClick(triage)}
                >
                  <td>{triage.id}</td>
                  <td>{triage.patient?.name || 'N/A'}</td>
                  <td>{triage.patientArrival?.arrival ? new Date(triage.patientArrival.arrival).toLocaleString() : 'N/A'}</td>
                  <td>{triage.symptoms || 'N/A'}</td>
                  <td>{triage.arterialPressure || 'N/A'}</td>
                  <td>{triage.weight}</td>
                  <td>{triage.height}</td>
                  {/* <td>{triage.kindId}</td> */}
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
};

export default Triages;
