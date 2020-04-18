
export const token = () => localStorage.getItem('datings-token');
export const setToken = value => localStorage.setItem('datings-token', value);
export const deleteToken = () => localStorage.removeItem('datings-token');

export const nickname = () => localStorage.getItem('datings-nickname');
export const setNickname = value => localStorage.setItem('datings-nickname', value);
