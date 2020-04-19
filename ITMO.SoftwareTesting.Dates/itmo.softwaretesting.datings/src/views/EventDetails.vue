<template>
	<div>
		<div class="d-flex justify-content-end">
			<b-button-group class="mb-3">
				<b-button variant="success" v-if="isFavorite" v-on:click="move(true)">Добавить в любимые</b-button>
				<b-button variant="danger" v-else v-on:click="move(false)">Удалить из любимых</b-button>
			</b-button-group>
		</div>
		<b-card
			:title="eventDetails.title"
			:sub-title="eventDetails.tagline"
			:img-src="eventDetails.images[0].image"
			img-alt="Image"
			img-top
		>
			<h6 v-for="date in eventDetails.dates">
				{{ new Date(date.start * 1000 + 10800000).toLocaleString('ru-RU') }}
				–
				{{ new Date(date.end * 1000 + 10800000).toLocaleString('ru-RU') }}
			</h6>

			<template v-if="eventDetails.place">
				<div v-if="eventDetails.place.title" class="py-2">
					<span>{{ eventDetails.place.title }}</span> <br>
					<span>{{ eventDetails.place.address }}, {{ eventDetails.place.subway }}</span>
				</div>
				<b-spinner variant="success" small v-else/>
			</template>

			<b-card-text class="text-justify" v-html="eventDetails.description"/>
			<b-card-text class="text-justify" v-html="eventDetails.body_text"/>
		</b-card>
	</div>
</template>
<script>
	export default {
		name: 'event-details',
		props: {
			eventDetails: {},
			favorites: Array,
		},

		computed: {
			isFavorite() {
				return this.favorites.includes((this.eventDetails || {id: -1}).id);
			},
		},

		methods: {
			move(value) {
				if (value) {
					this.favorites = [this.eventDetails.id, ...this.favorites];
				} else {
					this.favorites = this.favorites.filter(x => x !== this.eventDetails.id);
				}

				this.$emit('movedfavorite', value);
			},
		},
	};
</script>
