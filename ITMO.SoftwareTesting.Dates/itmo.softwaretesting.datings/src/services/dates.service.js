import {http} from '../tools/axios';

export const listDates = () =>
	http.get('api/dates').then(x => x.data);

export const createDate = (group, event) =>
	http.post(`api/dates/create/${group}/${event}`);
