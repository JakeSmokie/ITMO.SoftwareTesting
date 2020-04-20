import {kudaGo} from '../tools/axios';

export const eventCategories = () =>
	kudaGo.get('event-categories').then(x => x.data).then(x => [{name: ''}, ...x]);

export const locations = () =>
	kudaGo.get('locations').then(x => x.data).then(x => [{name: ''}, ...x]);

export const listEvents = (location, category) => {
	let url = 'events?';

	if (location) {
		url += 'location=' + location;
	}

	if (category) {
		url += '&categories=' + category;
	}

	return kudaGo.get(url).then(x => x.data.results);
};

export const eventDetails = (eventId) =>
	kudaGo.get(`events/${eventId}`).then(x => x.data);

export const placeDetails = (place) =>
	kudaGo.get(`places/${place}`).then(x => x.data);
