import React, { useState, useEffect } from 'react';
import axios from 'axios';

const Entry = () => {
  const [patients, setPatients] = useState([]);
  const [selectedPatient, setSelectedPatient] = useState(null);
  const [formData, setFormData] = useState({
    patientId: 0,
  });
  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const [apiError, setApiError] = useState('');

  useEffect(() => {
    const fetchPatients = async () => {
      setLoading(true);
      try {
        const response = await axios.get('http://localhost:5000/api/patients?withArrival=true');
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

  const handleSelectPatient = (patient) => {
    setSelectedPatient(patient);
    setFormData({ ...formData, patientId: patient.id });
    setErrors({});
    setApiError('');
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
    setErrors({ ...errors, [name]: '' });
  };

  const validateForm = () => {
    const newErrors = {};
    if (!formData.patientId) {
      newErrors.patientId = 'Selecione um paciente';
    }

 if (!formData.arterialPressure) {
      newErrors.arterialPressure = 'Campo obrigatório';
    }

    return newErrors;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const validationErrors = validateForm();
    if (Object.keys(validationErrors).length > 0) {
      setErrors(validationErrors);
      return;
    }

    const patientData = {
      patientId: parseInt(formData.patientId),
      patientArrivalId: selectedPatient.patientArrivalId,
      symptoms: formData.symptoms,
      arterialPressure : formData.arterialPressure,
      weight: formData.weight,
      height: formData.height,
      kind: parseInt(1)
    };

    setLoading(true);
    try {
      await axios.post('http://localhost:5000/api/triages', patientData, {
        headers: { 'Content-Type': 'application/json' },
      });
      alert('Criado com sucesso');
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
      <h2 className="text-2xl font-bold mb-6 text-center">Triagem de Pacientes</h2>

      <div className='mb-4'>
        <a href='/atendimento/em-andamento' className='btn btn-default'>Em andamento</a>
        <a href='/triagens' className='btn btn-ghost'>Triagem</a>
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
            ? `Criar Triagem para ${selectedPatient.name || 'Patient #' + selectedPatient.id}`
            : 'Criar Triagem'}
        </h3>
        <form onSubmit={handleSubmit} className="space-y-4">
          {/* Patient Selection */}

          <fieldset className="fieldset">
            <legend className="fieldset-legend">Paciente</legend>
            <input type="text" value={selectedPatient ? `${selectedPatient.name || 'N/A'} (${selectedPatient.email})` : 'Selecione um paciente'} className="input input-bordered w-full" disabled />
            {errors.patientId && <span className="text-error text-sm">{errors.patientId}</span>}
          </fieldset>


          <fieldset className="fieldset">
            <legend className="fieldset-legend">Sintomas</legend>
            <input
              name="symptoms"
              value={formData.symptoms}
              onChange={handleChange}
              className="input input-bordered w-full"
            />
            {errors.symptoms && <span className="text-error text-sm">{errors.symptoms}</span>}

          </fieldset>

          <fieldset className="fieldset">
            <legend className="fieldset-legend">Pressão Arterial</legend>
            <input
              name="arterialPressure"
              value={formData.arterialPressure}
              onChange={handleChange}
              className="input input-bordered w-full"
              placeholder='12/8'
            />
            {errors.arterialPressure && <span className="text-error text-sm">{errors.arterialPressure}</span>}

          </fieldset>

          <fieldset className="fieldset">
            <legend className="fieldset-legend">Peso</legend>
            <input
              name="weight"
              value={formData.weight}
              onChange={handleChange}
              className="input input-bordered w-full"
              placeholder='Em Kg'
            />
            {errors.weight && <span className="text-error text-sm">{errors.weight}</span>}

          </fieldset>

          <fieldset className="fieldset">
            <legend className="fieldset-legend">Altura</legend>
            <input
              name="height"
              value={formData.height}
              onChange={handleChange}
              className="input input-bordered w-full"
              placeholder='Em Metros'
            />
            {errors.weight && <span className="text-error text-sm">{errors.weight}</span>}

          </fieldset>





          <div className="form-control mt-6">
            <button
              type="submit"
              className="btn btn-primary w-full"
              disabled={loading || !selectedPatient}
            >
              {loading ? 'Submitting...' : 'Criar Triagem'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default Entry;
