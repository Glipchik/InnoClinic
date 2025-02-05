import React, { useContext, useEffect, useState } from 'react';
import { useFormik } from 'formik';
import { GET } from '../../shared/api/specializationApi';
import { AxiosResponse } from 'axios';
import Specialization from '../../entities/specialization';
import { UserManagerContext } from '../../shared/contexts/UserManagerContext';

function CreateAppointmentForm() {
  const [specializations, setSpecializations] = useState<Specialization[]>([]);
    const userManager = useContext(UserManagerContext);

  useEffect(() => {
    if (userManager) {
      async function getAccessToken() {
        const user = await userManager!.getUser();
        return user?.access_token ?? null;
      }

      const fetchSpecializations = async () => {
        try {
          const token = await getAccessToken();

          if (token === null) {
            return null;
          }
          
          const response: AxiosResponse<Specialization[]> = await GET(null, token);
          setSpecializations(response.data);
        } catch (error) {
          console.error('Error fetching specializations', error);
        }
      };

      fetchSpecializations();
    }
  }, [userManager]);

  const formik = useFormik({
    initialValues: {
      email: '',
      specialization: '',
    },
    onSubmit: values => {
      alert(JSON.stringify(values, null, 2));
    },
  });

  return (
    <form onSubmit={formik.handleSubmit}>
      <label htmlFor="email">Email Address</label>
      <input
        id="email"
        name="email"
        type="email"
        onChange={formik.handleChange}
        value={formik.values.email}
      />

      <label htmlFor="specialization">Specialization</label>
      <select
        id="specialization"
        name="specialization"
        onChange={formik.handleChange}
        value={formik.values.specialization}
      >
        <option value="" label="Select specialization" />
        {specializations.map((spec) => (
          <option key={spec.id} value={spec.id} label={spec.specializatioName} />
        ))}
      </select>

      <button type="submit">Submit</button>
    </form>
  );
};

export { CreateAppointmentForm }