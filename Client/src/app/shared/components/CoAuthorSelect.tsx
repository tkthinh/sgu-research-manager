import { useState, useEffect } from 'react';
import { Autocomplete, TextField, Box, Typography } from '@mui/material';
import { debounce } from 'lodash';
import apiClient from '../../../lib/api/api';

interface User {
  id: string;
  fullName: string;
  userName: string;
  departmentName: string;
}

interface CoAuthorSelectProps {
  value: string[];
  onChange: (newValue: string[]) => void;
  error?: boolean;
  helperText?: string;
}

export default function CoAuthorSelect({ value, onChange, error, helperText }: CoAuthorSelectProps) {
  const [inputValue, setInputValue] = useState('');
  const [options, setOptions] = useState<User[]>([]);
  const [selectedUsers, setSelectedUsers] = useState<User[]>([]);

  useEffect(() => {
    // Khi component mount, lấy thông tin của các user đã được chọn
    const fetchSelectedUsers = async () => {
      try {
        const promises = value.map(id =>
          apiClient.get<{ data: User }>(`/api/users/${id}`).then(res => res.data.data)
        );
        const users = await Promise.all(promises);
        setSelectedUsers(users);
      } catch (error) {
        console.error('Error fetching selected users:', error);
      }
    };

    if (value.length > 0) {
      fetchSelectedUsers();
    }
  }, [value]);

  const searchUsers = debounce(async (searchTerm: string) => {
    if (!searchTerm) {
      setOptions([]);
      return;
    }

    try {
      const response = await apiClient.get<{ data: User[] }>(`/api/users/search?searchTerm=${searchTerm}`);
      setOptions(response.data.data);
    } catch (error) {
      console.error('Error searching users:', error);
      setOptions([]);
    }
  }, 300);

  const handleInputChange = (_event: React.SyntheticEvent, newInputValue: string) => {
    setInputValue(newInputValue);
    searchUsers(newInputValue);
  };

  const handleChange = (_event: React.SyntheticEvent, newValue: User[]) => {
    setSelectedUsers(newValue);
    onChange(newValue.map(user => user.id));
  };

  return (
    <Autocomplete
      multiple
      options={options}
      value={selectedUsers}
      onChange={handleChange}
      inputValue={inputValue}
      onInputChange={handleInputChange}
      getOptionLabel={(option) => option.fullName}
      isOptionEqualToValue={(option, value) => option.id === value.id}
      renderOption={(props, option) => (
        <Box component="li" {...props}>
          <Box>
            <Typography variant="body1">
              {option.fullName} - {option.userName}
            </Typography>
            <Typography variant="body2" color="text.secondary">
              {option.departmentName}
            </Typography>
          </Box>
        </Box>
      )}
      renderInput={(params) => (
        <TextField
          {...params}
          label="Đồng tác giả"
          error={error}
          helperText={helperText}
          placeholder="Nhập tên hoặc tên đăng nhập để tìm kiếm"
        />
      )}
    />
  );
} 