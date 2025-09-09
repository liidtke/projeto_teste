import React, { useState, useEffect } from 'react';
import axios from 'axios';

const InProgress = () => {
  // State for patient arrivals
  const [patientArrivals, setPatientArrivals] = useState([]);
  // State for loading and error messages
  const [loading, setLoading] = useState(false);
  const [apiError, setApiError] = useState('');
  const [currentNumber, setCurrentNumber] = useState(0)

  useEffect(() => {
    const fetchInProgressArrivals = async () => {
      setLoading(true);
      try {
        const response = await axios.get('http://localhost:5000/api/patientarrivals?status=0');
        setPatientArrivals(response.data);
      } catch (error) {
        console.error('Erro ao buscar:', error);
        setApiError('Erro ao carregar.');
      } finally {
        setLoading(false);
      }
    };
    fetchInProgressArrivals();
    handleCurrentNumber();
  }, []);


  const handleCurrentNumber = async () => {
    console.log('HERE')
    try {
      const response = await axios.get(`http://localhost:5000/api/patientarrivals/current`, {
        headers: { 'Content-Type': 'application/json' },
      });
      setCurrentNumber(response.data?.sequentialNumber)
    } catch (error) {
      console.error('Error updating patient arrival:', error.response?.data || error.message);
    }

  }

  const nextNumber = async () => {
    console.log('HERE')
    try {
      const response = await axios.get(`http://localhost:5000/api/patientarrivals/next`, {
        headers: { 'Content-Type': 'application/json' },
      });
      setCurrentNumber(response.data?.sequentialNumber)
    } catch (error) {
      console.error('Error updating patient arrival:', error.response?.data || error.message);
    }

  }

  // Handle clicking a patient arrival to update status
  const handleArrivalClick = async (arrival) => {
    const updatedArrival = {
      id: arrival.id,
      sequentialNumber: arrival.sequentialNumber,
      patientId: arrival.patientId,
      patient: arrival.patient, // Include if required by API
      arrival: arrival.arrival, // ISO string
      status: 1, // Update to Completed
    };

    try {
      await axios.put(`http://localhost:5000/api/patientarrivals/${arrival.id}`, updatedArrival, {
        headers: { 'Content-Type': 'application/json' },
      });
      // Remove the updated arrival from the list
      setPatientArrivals(patientArrivals.filter((pa) => pa.id !== arrival.id));
      alert('Atendimento Concluído!');
    } catch (error) {
      console.error('Error updating patient arrival:', error.response?.data || error.message);
      setApiError('Failed to update patient arrival.');
    }
  };

  return (
    <div className="max-w-5xl mx-auto p-6 bg-base-100 shadow-xl rounded-lg">
      <h2 className="text-2xl font-bold mb-6 text-center">Pacientes Pendentes</h2>


      <div className='mb-0'>
        <a href='/atendimento/' className='btn btn-default'>Criar Atendimento</a>
        <a href='/pacientes' className='btn btn-ghost'>Criar Paciente</a>
        <button className='btn btn-secondary' onClick={nextNumber}>Próximo Número</button>

        <div className='flex w-1/2 mt-8'>
          <a className='font-semibold'>Número Atual: {currentNumber}</a>
        </div>
      </div>
      <div className='divider mb-4'></div>

      {loading && <p className="text-info">Carregando...</p>}
      {apiError && <p className="text-error">{apiError}</p>}
      {patientArrivals.length === 0 && !loading && !apiError && (
        <p className="text-warning">Nada para exibir.</p>
      )}

      {patientArrivals.length > 0 && (
        <div className="overflow-x-auto">
          <table className="table w-full">
            <thead>
              <tr>
                <th>Número</th>
                <th>Nome</th>
                <th>Email</th>
                <th>Data/Hora</th>
                <th>Status</th>
                <th>Ação</th>
              </tr>
            </thead>
            <tbody>
              {patientArrivals.map((arrival) => (
                <tr
                  key={arrival.id}
                 className={`hover cursor-pointer ${arrival.sequentialNumber == currentNumber ? 'border-4 border-red-300 rounded-lg' : ''}`}
                  onClick={() => handleArrivalClick(arrival)}
                >
                  <td>{arrival.sequentialNumber}</td>
                  <td>{arrival.patient?.name || 'N/A'}</td>
                  <td>{arrival.patient?.email || 'N/A'}</td>
                  <td>{new Date(arrival.arrival).toLocaleString()}</td>
                  <td>{arrival.status == 0 ? 'Pendente' : "Completo"}</td>
                  <td>Concluir</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
};

export default InProgress;
