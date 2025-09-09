import React, { useState, useEffect } from 'react';
import axios from 'axios';

const Entry = () => {
  const [patients, setPatients] = useState([]);
  const [selectedPatient, setSelectedPatient] = useState(null);
  const [formData, setFormData] = useState({
    patientId: 0,
    arrival: new Date().toISOString().slice(0, 16), // Default to current date/time
    status: 'Pending', // Default status
  });
  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const [apiError, setApiError] = useState('');

  useEffect(() => {
    const fetchPatients = async () => {
      setLoading(true);
      try {
        const response = await axios.get('http://localhost:5000/api/patients');
        setPatients(response.data);
      } catch (error) {
        console.error('Erro ao buscar:', error);
        setApiError('Erro ao carregar.');
      } finally {
        setLoading(false);
      }
    };
    fetchPatients();
  }, []);

  // Handle patient selection
  const handleSelectPatient = (patient) => {
    setSelectedPatient(patient);
    setFormData({ ...formData, patientId: patient.id });
    setErrors({});
    setApiError('');
  };

  // Handle form input changes
  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
    setErrors({ ...errors, [name]: '' });
  };

  // Validate form
  const validateForm = () => {
    const newErrors = {};
    if (!formData.patientId) {
      newErrors.patientId = 'Selecione um paciente';
    }
    if (!formData.status) {
      newErrors.status = 0
    }else{
      formData.status = 0;
    }
    return newErrors;
  };

  // Handle form submission
  const handleSubmit = async (e) => {
    e.preventDefault();
    const validationErrors = validateForm();
    if (Object.keys(validationErrors).length > 0) {
      setErrors(validationErrors);
      return;
    }

    const patientArrivalData = {
      patientId: parseInt(formData.patientId),
      arrival: new Date(formData.arrival).toISOString(), // Ensure ISO format
      status: parseInt(formData.status),
    };

    setLoading(true);
    try {
      let result = await axios.post('http://localhost:5000/api/patientarrivals', patientArrivalData, {
        headers: { 'Content-Type': 'application/json' },
      });
      console.log(result);
      alert('Número de Atendimento: ' + result.data.sequentialNumber);
      // Reset form
      setFormData({
        patientId: 0,
        arrival: new Date().toISOString().slice(0, 16),
        status: 'Pending',
      });
      setSelectedPatient(null);
    } catch (error) {
      console.error('Erro ao criar:', error.response?.data || error.message);
      setApiError('Erro ao criar');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="max-w-4xl mx-auto p-6 bg-base-100 shadow-xl rounded-lg">
      <h2 className="text-2xl font-bold mb-6 text-center">Atendimento de Pacientes</h2>

      <div className='mb-4'>
      <a href='/atendimento/em-andamento' className='btn btn-default'>Em andamento</a>
      <a href='/atendimento/completos' className='btn btn-ghost'>Triagem</a>
      <a href='/pacientes' className='btn btn-ghost'>Criar Paciente</a>
      </div>
      <div className='divider mb-4'></div>

      {/* Patients List */}
      <div className="mb-8">
        <h3 className="text-xl font-semibold mb-4">Pacientes</h3>
        {loading && <p className="text-info">Carregando...</p>}
        {apiError && !loading && <p className="text-error">{apiError}</p>}
        {patients.length === 0 && !loading && !apiError && (
          <p className="text-warning">Nada para exibir.</p>
        )}
        {patients.length > 0 && (
          <div className="overflow-x-auto">
            <table className="table w-full">
              <thead>
                <tr>
                  <th>Id</th>
                  <th>Nome</th>
                  <th>Email</th>
                  <th>Telefone</th>
                  <th>Sexo</th>
                  <th>Ação</th>
                </tr>
              </thead>
              <tbody>
                {patients.map((patient) => (
                  <tr
                    key={patient.id}
                    className={selectedPatient?.id === patient.id ? 'bg-base-200' : ''}
                  >
                    <td>{patient.id}</td>
                    <td>{patient.name || 'N/A'}</td>
                    <td>{patient.email}</td>
                    <td>{patient.phone || 'N/A'}</td>
                    <td>{patient.gender == 0 ? "Masculino" : "Feminino"}</td>
                    <td>
                      <button
                        className="btn btn-sm btn-primary"
                        onClick={() => handleSelectPatient(patient)}
                      >
                        Selecionar
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>

      <div>
        <h3 className="text-xl font-semibold mb-4">
          {selectedPatient
            ? `Criar Atendimento para ${selectedPatient.name || 'Patient #' + selectedPatient.id}`
            : 'Criar Atendimento'}
        </h3>
        <form onSubmit={handleSubmit} className="space-y-4">
          {/* Patient Selection */}

    <fieldset className="fieldset">
          <legend className="fieldset-legend">Paciente</legend>
            <input type="text" value={selectedPatient ? `${selectedPatient.name || 'N/A'} (${selectedPatient.email})` : 'Selecione um paciente'} className="input input-bordered w-full" disabled />
            {errors.patientId && <span className="text-error text-sm">{errors.patientId}</span>}
        </fieldset>

          <fieldset className="fieldset">
          <legend className="fieldset-legend">Status</legend>
            <select
              name="status"
              value={formData.status}
              onChange={handleChange}
              className="select select-bordered w-full"
            >
              <option value="0">Pendente</option>
              {/* <option value="1">Completo</option> */}
            </select>
            {errors.status && <span className="text-error text-sm">{errors.status}</span>}

        </fieldset>

          <div className="form-control mt-6">
            <button
              type="submit"
              className="btn btn-primary w-full"
              disabled={loading || !selectedPatient}
            >
              {loading ? 'Submitting...' : 'Criar Atendimento'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default Entry;
