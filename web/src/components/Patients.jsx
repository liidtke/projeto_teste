import React, { useState } from 'react';
import axios from 'axios';

export default function Patients() {
  // Form state
  const [formData, setFormData] = useState({
    id: 0, 
    name: '',
    phone: '',
    email: '',
    gender: 0 
  });

  // Error state
  const [errors, setErrors] = useState({});

  // Handle input changes
  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
    setErrors({ ...errors, [name]: '' });
  };

  // Validate form
  const validateForm = () => {
    const newErrors = {};
    if (!formData.name || formData.name.length > 200) {
      newErrors.name = 'Nome inválido';
    }
    if (!formData.phone || formData.phone.length > 50) {
      newErrors.phone = 'Telefone inválido ';
    }
    if (!formData.email) {
      newErrors.email = 'Email é obrigatório';
    } else if (formData.email.length > 100) {
      newErrors.email = 'Email inválido';
    } else if (!/\S+@\S+\.\S+/.test(formData.email)) {
      newErrors.email = 'Email inválido';
    }
    return newErrors;
  };

  function test (){
    console.log('test')
  }
  // Handle form submission
  const handleSubmit = async () => {
    console.log('here')
    // e.preventDefault();
    const validationErrors = validateForm();
    if (Object.keys(validationErrors).length > 0) {
      setErrors(validationErrors);
      return;
    }

    // Prepare the body matching the C# Patient class
    const patientData = {
      id: parseInt(formData.id), // Ensure Id is an integer
      name: formData.name || null, // Handle nullable Name
      phone: formData.phone || null, // Handle nullable Phone
      email: formData.email, // Required
      gender: parseInt(formData.gender) ?? 0, // Enum value as string
    };

    try {
      // Send POST request to your API endpoint
      const response = await axios.post('http://localhost:5000/api/patients', patientData, {
        headers: {
          'Content-Type': 'application/json',
        },
      });
      console.log('Success:', response.data);
      alert('Paciente Criado com sucesso!');
      // Reset form
      setFormData({ id: 0, name: '', phone: '', email: '', gender: 0 });
    } catch (error) {
      console.error('Error:', error.response?.data || error.message);
      alert('Erro ao criar paciente');
    }
  };

  return (
    <div className="max-w-md mx-auto p-6 bg-base-100 shadow-xl rounded-lg">
      <h2 className="text-2xl font-bold mb-6 text-center">Criar Paciente</h2>


      <div className='mb-4'>
      <a href='/atendimento' className='btn btn-default'>Atendimento</a>
      </div>
      <div className='divider mb-4'></div>

      <div  className="space-y-4">

        <fieldset className="fieldset">
          <legend className="fieldset-legend">Nome</legend>
          <input type="text" name="name" value={formData.name} onChange={handleChange} className="input input-bordered w-full" placeholder="Nome" maxLength={50} />
          {errors.name && <span className="text-error text-sm">{errors.name}</span>}
        </fieldset>

        <fieldset className="fieldset">
          <legend className="fieldset-legend">Telefone</legend>
          <input type="text" name="phone" value={formData.phone} onChange={handleChange} className="input input-bordered w-full" placeholder="(19) 99999999" maxLength={50} />
          {errors.phone && <span className="text-error text-sm">{errors.phone}</span>}
        </fieldset>

        <fieldset className="fieldset">
          <legend className="fieldset-legend">Email</legend>
          <input type="email" name="email" value={formData.email} onChange={handleChange} className="input input-bordered w-full" placeholder="Email" maxLength={100} required />
          {errors.email && <span className="text-error text-sm">{errors.email}</span>}
        </fieldset>

        <fieldset className="fieldset">
          <legend className="fieldset-legend">Sexo</legend>
          <select
            name="gender"
            value={formData.gender}
            onChange={handleChange}
            className="select select-bordered w-full"
          >
            <option value="0">Masculino</option>
            <option value="1">Feminino</option>
          </select>
          {errors.gender && <span className="text-error text-sm">{errors.gender}</span>}
        </fieldset>

        <div className="form-control mt-6">
          <button type="submit" className="btn btn-primary w-full" onClick={handleSubmit}>
            Salvar
          </button>
        </div>
      </div>
    </div>
  );
};

