import {http} from '../tools/axios';

export const favoriteEvents = () =>
	http.get('api/favorite-events').then(x => x.data);

export const addEventInFavorites = id =>
	http.post(`api/favorite-events/add/${id}`).then(x => x.data);

export const removeEventFromFavorites = id =>
	http.post(`api/favorite-events/remove/${id}`).then(x => x.data);


