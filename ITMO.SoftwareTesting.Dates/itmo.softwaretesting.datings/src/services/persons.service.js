import {http} from '../tools/axios';

export const searchPersons = (nickname) =>
	http.get(`api/persons/search/${encodeURIComponent(nickname)}`).then(x => x.data);
